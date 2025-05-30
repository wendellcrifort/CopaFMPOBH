﻿using Application.Common.Enum;
using Application.Common.Interfaces;
using Application.Hubs;
using Application.Services.Partida.Model;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Partida
{
    public class PartidaService : IPartidaService
    {
        private readonly IMapper _mapper;
        private readonly ICopaDbContext _copaDbContext;
        private readonly IHubContext<PlacarHub> _hubContext;

        public PartidaService(IMapper mapper, ICopaDbContext copaDbContext, IHubContext<PlacarHub> hubContext)
        {
            _mapper = mapper;
            _copaDbContext = copaDbContext;
            _hubContext = hubContext;            
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
                                         .Include(i => i.TimeMandante!.Jogadores)
                                         .Include(i => i.TimeVisitante!.Jogadores)
                                         .Where(x => !string.IsNullOrEmpty(data)
                                                      ? x.DataPartida == data
                                                      : true)
                                         .OrderBy(o => o.DataHoraPartida)
                                         .ToListAsync();

            var eventos = await BuscarEventosPartidas(partidas.Select(x => x.Id).ToList());

            var partidasModel = _mapper.Map<List<PartidaViewModel>>(partidas);

            foreach (var partida in partidasModel)
                partida.Eventos = eventos.Where(x => x.IdPartida == partida.IdPartida).ToList();

            return partidasModel;
        }

        public async Task<PartidasHomeViewModel> BuscarPartidasHome()
        {
            var partidas = await _copaDbContext.Partida
                                         .AsNoTracking()
                                         .Include(i => i.TimeMandante!.Jogadores)
                                         .Include(i => i.TimeVisitante!.Jogadores)
                                         .OrderBy(o => o.DataHoraPartida)
                                         .ToListAsync();

            var eventos = await BuscarEventosPartidas(partidas.Select(x => x.Id).ToList());

            var partidasHome = new PartidasHomeViewModel();

            partidasHome.PartidasAoVivo = _mapper.Map<List<PartidaViewModel>>(partidas.Where(x => x.EmAndamento));
            partidasHome.ProximaPartida = _mapper.Map<PartidaViewModel>(partidas.FirstOrDefault(x => !x.EmAndamento && !x.PartidaFinalizada));
            partidasHome.PartidasEncerradas = _mapper.Map<List<PartidaViewModel>>(partidas.Where(x => x.PartidaFinalizada).OrderByDescending(x=>x.DataHoraPartida).Take(4));

            foreach (var partida in partidasHome.PartidasAoVivo)
                partida.Eventos = eventos.Where(x => x.IdPartida == partida.IdPartida).ToList();

            foreach (var partida in partidasHome.PartidasEncerradas)
                partida.Eventos = eventos.Where(x => x.IdPartida == partida.IdPartida).ToList();

            return partidasHome;
        }

        public async Task<PartidaViewModel> BuscarPartidaEmAndamento(int idPartida)
        {
            var dadosPartida = await _copaDbContext.Partida
                                                   .AsNoTracking()
                                                   .Include(i => i.TimeMandante)
                                                      .ThenInclude(i => i.Jogadores.OrderBy(o => o.Numero))
                                                   .Include(i => i.TimeVisitante)
                                                      .ThenInclude(i => i.Jogadores.OrderBy(o => o.Numero))
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

        private async Task<List<EventosPartidaViewModel>> BuscarEventosPartidas(List<int> idsPartida)
        {
            var eventos = await _copaDbContext.EventosPartida
                                              .AsNoTracking()
                                              .Where(x => idsPartida.Contains(x.IdPartida))
                                              .ToListAsync();

            return _mapper.Map<List<EventosPartidaViewModel>>(eventos);
        }

        public async Task FinalizarPartida(int idPartida)
        {
            var partida = await _copaDbContext.Partida.FirstAsync(x => x.Id == idPartida);

            partida.EmAndamento = false;
            partida.PartidaFinalizada = true;

            var partidadeEncerrada = _mapper.Map<Domain.Entities.Partida>(partida);
            _copaDbContext.Partida.Update(partida);
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
            else if (evento == TipoEventoEnum.GolMarcado)
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
                case TipoEventoEnum.MelhorGoleiro:
                    await AddVotoMelhorGoleiro(idJogador);
                    break;
                case TipoEventoEnum.MelhorJogador:
                    await AddVotoMelhorJogador(idJogador);
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

            await _hubContext.Clients.All.SendAsync("ReceberAtualizacaoPlacar", idPartida, partida.GolsTimeMandante ?? 0, partida.GolsTimeVisitante ?? 0);

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

            await _hubContext.Clients.All.SendAsync("ReceberAtualizacaoPlacar", partida.Id, partida.GolsTimeMandante ?? 0, partida.GolsTimeVisitante ?? 0);

            return new EventoPlacarViewModel()
            {
                IdEvento = idEvento,
                GolsMandante = partida.GolsTimeMandante ?? 0,
                GolsVisitante = partida.GolsTimeVisitante ?? 0,
                EventoDeletado = result > 0
            };
        }

        public async Task SalvarSumula(SumulaModel sumula)
        {
            _copaDbContext.Sumula.Add(_mapper.Map<Sumula>(sumula));
            await _copaDbContext.SaveChangesAsync();
        }
        public async Task<SumulaModel> BuscarSumula(int idPartida)
        {
            var sumula = await _copaDbContext.Sumula
                                             .Where(x=>x.IdPartida == idPartida)
                                             .OrderByDescending(x=>x.Id)
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync();

            var retorno = _mapper.Map<SumulaModel>(sumula);
            return retorno;
        }

        private async Task RemoverCartaoVermelho(Domain.Entities.Jogador jogador)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == jogador.IdTime);

            jogador.CartoesVermelhos -= 1;
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

        private async Task AddVotoMelhorJogador(int idJogador) 
        {
            var jogador = await _copaDbContext.Jogador.FirstAsync(f => f.Id == idJogador);
            jogador.MelhorJogador += 1;
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();

        }

        private async Task AddVotoMelhorGoleiro(int idJogador)
        {
            var jogador = await _copaDbContext.Jogador.FirstAsync(f => f.Id == idJogador);
            jogador.MelhorGoleiro += 1;
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();

        }

        private async Task AddCartaoVermelho(Domain.Entities.Jogador jogador, Domain.Entities.Time time)
        {
            jogador.CartoesVermelhos += 1;
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

        private async Task SalvarScoreVisitante(Domain.Entities.Partida partida)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdTimeVisitante);

            var pontos = partida.GolsTimeVisitante > partida.GolsTimeMandante
                                                            ? 3
                                                                : partida.GolsTimeMandante == partida.GolsTimeVisitante
                                                            ? 1
                                                                : 0;

            time.Pontos += pontos;
            AddResultado(time, pontos);

            _copaDbContext.Time.Update(time);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task SalvarScoreMandante(Domain.Entities.Partida partida)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdTimeMandante);

            var pontos = partida.GolsTimeMandante > partida.GolsTimeVisitante
                                ? 3
                                    : partida.GolsTimeMandante == partida.GolsTimeVisitante
                                ? 1
                                    : 0;

            time.Pontos += pontos;
            AddResultado(time, pontos);

            _copaDbContext.Time.Update(time);
            await _copaDbContext.SaveChangesAsync();
        }

        private static void AddResultado(Domain.Entities.Time time, int pontos)
        {
            switch (pontos)
            {
                case 3:
                    time.Vitorias += 1;
                    break;
                case 1:
                    time.Empates += 1;
                    break;
                case 0:
                    time.Derrotas += 1;
                    break;
                default:
                    break;
            }
        }
    }
}
