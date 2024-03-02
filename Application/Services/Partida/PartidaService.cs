using Application.Common.Enum;
using Application.Common.Interfaces;
using Application.Services.Partida.Model;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace Application.Services.Partida
{
    public class PartidaService : IPartidaService
    {
        private readonly IMapper _mapper;
        private readonly ICopaDbContext _copaDbContext;

        public PartidaService(IMapper mapper, ICopaDbContext copaDbContext)
        {
            _mapper = mapper;
            _copaDbContext = copaDbContext;
        }

        public async Task<int> CriarPartida(PartidaModel partida)
        {
            var partidaModel = _mapper.Map<Domain.Entities.Partida>(partida);

            _copaDbContext.Partida.Add(partidaModel);
            return await _copaDbContext.SaveChangesAsync();
        }

        public async Task<int> IniciarPartida(int idPartida)
        {
            var partida = await _copaDbContext.Partida.FirstAsync(x => x.Id == idPartida);

            partida.EmAndamento = true;

            return await _copaDbContext.SaveChangesAsync();
        }

        public async Task<List<PartidaViewModel>> BuscarPartidas(string? data)
        {
            var partidas = await _copaDbContext.Partida
                                         .AsNoTracking()
                                         .Include(i => i.TimeMandante)
                                         .Include(i => i.TimeVisitante)
                                         .Where(x => !string.IsNullOrEmpty(data)
                                                      ? x.DataPartida == data
                                                      : true)
                                         .ToListAsync();

            return _mapper.Map<List<PartidaViewModel>>(partidas);
        }

        public async Task<PartidaViewModel> BuscarPartidaEmAndamento(int idPartida)
        {
            var dadosPartida = await _copaDbContext.Partida
                                                   .AsNoTracking()
                                                   .Include(i => i.TimeMandante)
                                                      .ThenInclude(i => i.Jogadores)
                                                   .Include(i => i.TimeVisitante)
                                                      .ThenInclude(i => i.Jogadores)
                                                   .FirstOrDefaultAsync(x => x.Id == idPartida);

            return _mapper.Map<PartidaViewModel>(dadosPartida);
        }

        public async Task<List<EventosPartidaViewModel>> BuscarEventosPartida(int idPartida)
        {
            var eventos = await _copaDbContext.EventosPartida
                                              .AsNoTracking()
                                              .Where(x => x.IdPartida == idPartida)
                                              .ToListAsync();

            return _mapper.Map<List<EventosPartidaViewModel>>(eventos);
        }

        public async Task FinalizarPartida(PartidaModel partida)
        {
            var partidadeEncerrada = _mapper.Map<Domain.Entities.Partida>(partida);
            _copaDbContext.Partida.Update(partidadeEncerrada);
            
            await _copaDbContext.SaveChangesAsync();

            await SalvarScoreMandante(partida);
            await SalvarScoreVisitante(partida);
        }

        public async Task<EventoPlacarViewModel> RegistrarEventoPartida(int idPartida,
                                                                        int idJogador,
                                                                        TipoEventoEnum evento,
                                                                        int? idGoleiro = 0)
        {
            var partida = await _copaDbContext.Partida.FirstAsync(x => x.Id == idPartida);
            var jogador = await _copaDbContext.Jogador.FirstAsync(x => x.Id == idJogador);
            var goleiro = await _copaDbContext.Jogador.FirstOrDefaultAsync(x => x.Id == idGoleiro);
            var timeAcao = await _copaDbContext.Time.FirstAsync(x => x.Id == jogador.IdTime);
            Domain.Entities.Time timePassivo = new Domain.Entities.Time();

            if (evento == TipoEventoEnum.GolContra)
            {
                if (partida.IdTimeMandante == goleiro.IdTime)
                    timePassivo = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdTimeVisitante);

                else
                    timePassivo = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdTimeMandante);
            }
            else if(evento == TipoEventoEnum.GolMarcado)            
                timePassivo = await _copaDbContext.Time.FirstAsync(x => x.Id == goleiro.IdTime);

            switch (evento)
            {
                case TipoEventoEnum.GolMarcado:
                    await AddGolMarcado(partida, jogador, timeAcao);
                    await AddGolSofrido(goleiro, timePassivo);
                    break;
                case TipoEventoEnum.GolContra:
                    await AddGolContra(goleiro, timeAcao, timePassivo, partida);
                    break;
                case TipoEventoEnum.CartaoAmarelo:
                    await AddCartaoAmarelo(jogador, timeAcao);
                    break;
                case TipoEventoEnum.CartaoVermelho:
                    await AddCartaoVermelho(jogador, timeAcao);
                    break;
                default:
                    break;
            }

            var novoEvento = new EventosPartida()
            {
                IdPartida = idPartida,
                IdJogador = idJogador,
                DescricaoEvento = evento.ToString(),
                IdGoleiro = goleiro?.Id,
                IdTime = timeAcao.Id,
            };

            _copaDbContext.EventosPartida.Add(novoEvento);
            await _copaDbContext.SaveChangesAsync();

            return new EventoPlacarViewModel()
            {
                IdEvento = novoEvento.Id,
                GolsMandante = partida.GolsTimeMandante ?? 0,
                GolsVisitante = partida.GolsTimeVisitante ?? 0,
            };
        }

        public async Task<EventoPlacarViewModel> RemoverEventoPartida(int idEvento)
        {

            var evento = await _copaDbContext.EventosPartida
                                             .FirstOrDefaultAsync(x => x.Id == idEvento)
                                                ?? throw new Exception("Evento não localizado");

            var partida = await _copaDbContext.Partida
                                              .FirstOrDefaultAsync(x => x.Id == evento.IdPartida)
                                                 ?? throw new Exception("Partida não localizada");

            var jogador = await _copaDbContext.Jogador
                                              .FirstOrDefaultAsync(x => x.Id == evento.IdJogador)
                                                 ?? throw new Exception("Jogador não localizado");

            if (evento.DescricaoEvento == TipoEventoEnum.CartaoAmarelo.ToString())
            {
                await RemoverCartaoAmarelo(jogador);
                return new EventoPlacarViewModel()
                {
                    IdEvento = idEvento,
                    GolsMandante = partida.GolsTimeMandante ?? 0,
                    GolsVisitante = partida.GolsTimeVisitante ?? 0
                };
            }
            else if (evento.DescricaoEvento == TipoEventoEnum.CartaoVermelho.ToString())
            {
                await RemoverCartaoVermelho(jogador);
                return new EventoPlacarViewModel()
                {
                    IdEvento = idEvento,
                    GolsMandante = partida.GolsTimeMandante ?? 0,
                    GolsVisitante = partida.GolsTimeVisitante ?? 0
                };
            }

            var goleiro = await _copaDbContext.Jogador.FirstAsync(x => x.Id == evento.IdGoleiro);

            if (evento.DescricaoEvento == TipoEventoEnum.GolMarcado.ToString())
            {
                await RemoverGolMarcado(jogador, partida);
                await RemoverGolSofrido(goleiro);
            }
            else if (evento.DescricaoEvento == TipoEventoEnum.GolContra.ToString())
                await RemoverGolContra(goleiro, partida);

            _copaDbContext.EventosPartida.Remove(evento);
            var result = await _copaDbContext.SaveChangesAsync();

            return new EventoPlacarViewModel()
            {
                IdEvento = idEvento,
                GolsMandante = partida.GolsTimeMandante ?? 0,
                GolsVisitante = partida.GolsTimeVisitante ?? 0,
                EventoDeletado = result > 0
            };
        }

        private async Task RemoverCartaoVermelho(Domain.Entities.Jogador jogador)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == jogador.IdTime);

            jogador.CartoesVemelhos -= 1;
            jogador.Suspenso = false;
            time.CartoesVermelhos -= 1;

            _copaDbContext.Time.Update(time);
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task RemoverCartaoAmarelo(Domain.Entities.Jogador jogador)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == jogador.IdTime);

            jogador.CartoesAmarelos -= 1;
            time.CartoesAmarelos -= 1;

            _copaDbContext.Time.Update(time);
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task RemoverGolSofrido(Domain.Entities.Jogador goleiro)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == goleiro.IdTime);

            goleiro.GolsSofridos -= 1;
            time.GolsSofridos -= 1;
            time.SaldoGols += 1;

            _copaDbContext.Time.Update(time);
            _copaDbContext.Jogador.Update(goleiro);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task RemoverGolContra(Domain.Entities.Jogador goleiro,
                                            Domain.Entities.Partida partida)
        {
            Domain.Entities.Time timeAdversario = new Domain.Entities.Time();
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == goleiro.IdTime);

            goleiro.GolsSofridos -= 1;
            time.GolsSofridos -= 1;
            time.SaldoGols += 1;

            if (partida.IdTimeMandante == time.Id)
            {
                partida.GolsTimeVisitante -= 1;
                timeAdversario = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdTimeVisitante);
                timeAdversario.GolsFeitos -= 1;
                timeAdversario.SaldoGols -= 1;
            }
            else
            {
                partida.GolsTimeMandante -= 1;
                timeAdversario = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdTimeMandante);
                timeAdversario.GolsFeitos -= 1;
                timeAdversario.SaldoGols -= 1;
            }

            _copaDbContext.Time.Update(time);
            _copaDbContext.Time.Update(timeAdversario);
            _copaDbContext.Jogador.Update(goleiro);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task RemoverGolMarcado(Domain.Entities.Jogador jogador, Domain.Entities.Partida partida)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == jogador.IdTime);

            if (partida.IdTimeMandante == time.Id)
                partida.GolsTimeMandante -= 1;
            else
                partida.GolsTimeVisitante -= 1;

            jogador.GolsMarcados -= 1;
            time.GolsFeitos -= 1;
            time.SaldoGols -= 1;

            _copaDbContext.Partida.Update(partida);
            _copaDbContext.Time.Update(time);
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddCartaoVermelho(Domain.Entities.Jogador jogador, Domain.Entities.Time time)
        {
            jogador.CartoesVemelhos += 1;
            jogador.Suspenso = true;

            time.CartoesVermelhos += 1;

            _copaDbContext.Time.Update(time);
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddCartaoAmarelo(Domain.Entities.Jogador jogador, Domain.Entities.Time time)
        {
            jogador.CartoesAmarelos += 1;

            if (jogador.CartoesAmarelos == 3)
            {
                jogador.Suspenso = true;
                jogador.CartoesAmarelos = 0;
            }

            time.CartoesAmarelos += 1;

            _copaDbContext.Time.Update(time);
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddGolSofrido(Domain.Entities.Jogador goleiro, Domain.Entities.Time time)
        {
            goleiro.GolsSofridos += 1;
            time.GolsSofridos += 1;
            time.SaldoGols -= 1;

            _copaDbContext.Time.Update(time);
            _copaDbContext.Jogador.Update(goleiro);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddGolContra(Domain.Entities.Jogador goleiro,
                                        Domain.Entities.Time timeAcao,
                                        Domain.Entities.Time timePassivo,
                                        Domain.Entities.Partida partida)
        {
            goleiro.GolsSofridos += 1;
            timeAcao.GolsSofridos += 1;
            timeAcao.SaldoGols -= 1;

            timePassivo.GolsFeitos += 1;
            timePassivo.SaldoGols += 1;

            if (partida.IdTimeMandante == timePassivo.Id)
                partida.GolsTimeMandante += 1;
            else
                partida.GolsTimeVisitante += 1;

            _copaDbContext.Partida.Update(partida);
            _copaDbContext.Time.Update(timeAcao);
            _copaDbContext.Time.Update(timePassivo);
            _copaDbContext.Jogador.Update(goleiro);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddGolMarcado(Domain.Entities.Partida partida, Domain.Entities.Jogador jogador, Domain.Entities.Time time)
        {
            AddGolPartida(partida, jogador);

            jogador.GolsMarcados += 1;
            time.GolsFeitos += 1;
            time.SaldoGols += 1;

            _copaDbContext.Jogador.Update(jogador);
            _copaDbContext.Time.Update(time);
            _copaDbContext.Partida.Update(partida);

            await _copaDbContext.SaveChangesAsync();
        }

        private static void AddGolPartida(Domain.Entities.Partida partida, Domain.Entities.Jogador jogador)
        {
            if (partida.IdTimeMandante == jogador.IdTime)
                partida.GolsTimeMandante += 1;
            else
                partida.GolsTimeVisitante += 1;
        }

        private async Task SalvarScoreVisitante(PartidaModel partida)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdVisitante);

            var pontos = partida.GolsVisitante > partida.GolsMandante ? 3 : partida.GolsMandante == partida.GolsVisitante ? 1 : 0;

            time.Pontos += pontos;

            _copaDbContext.Time.Update(time);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task SalvarScoreMandante(PartidaModel partida)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdMandante);

            var pontos = partida.GolsMandante > partida.GolsVisitante ? 3 : partida.GolsMandante == partida.GolsVisitante ? 1 : 0;

            time.Pontos += pontos;

            _copaDbContext.Time.Update(time);
            await _copaDbContext.SaveChangesAsync();
        }

    }
}
