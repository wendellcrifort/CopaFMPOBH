using Application.Common.Enum;
using Application.Common.Interfaces;
using Application.Services.Partida.Model;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            _copaDbContext.Partida.Update(_mapper.Map<Domain.Entities.Partida>(partida));

            var cartoes = await _copaDbContext.EventosPartida.AsNoTracking()
                                                             .Where(x => x.IdPartida == partida.IdPartida
                                                                    && (x.DescricaoEvento == TipoEventoEnum.CartaoAmarelo.ToString()
                                                                    || x.DescricaoEvento == TipoEventoEnum.CartaoAmarelo.ToString()))
                                                             .ToListAsync();

            await SalvarScoreMandante(partida, cartoes);
            await SalvarScoreVisitante(partida, cartoes);
        }

        public async Task<int> RegistrarEventoPartida(int idPartida, int idJogador, TipoEventoEnum evento, int idGoleiro)
        {
            switch (evento)
            {
                case TipoEventoEnum.GolMarcado:
                    await AddGolMarcado(idJogador);
                    await AddGolSofrido(idGoleiro);
                    break;
                case TipoEventoEnum.GolContra:
                    await AddGolSofrido(idGoleiro);
                    break;
                case TipoEventoEnum.CartaoAmarelo:
                    await AddCartaoAmarelo(idJogador);
                    break;
                case TipoEventoEnum.CartaoVermelho:
                    await AddCartaoVermelho(idJogador);
                    break;
                default:
                    break;
            }

            var novoEvento = new EventosPartida()
            {
                IdPartida = idPartida,
                IdJogador = idJogador,
                DescricaoEvento = evento.ToString(),
            };

            _copaDbContext.EventosPartida.Add(novoEvento);
            await _copaDbContext.SaveChangesAsync();

            return novoEvento.Id;
        }

        public async Task<Task> RemoverEventoPartida(int idEvento)
        {
            var evento = _copaDbContext.EventosPartida
                                             .AsNoTracking()
                                             .First(x => x.Id == idEvento);

            Domain.Entities.Jogador jogador = await _copaDbContext.Jogador.FirstAsync(x => x.Id == evento.IdJogador);

            if (evento.DescricaoEvento == TipoEventoEnum.CartaoAmarelo.ToString())
            {
                await RemoverCartaoAmarelo(jogador);
                return Task.CompletedTask;
            }
            else if (evento.DescricaoEvento == TipoEventoEnum.CartaoVermelho.ToString())
            {
                await RemoverCartaoVermelho(jogador);
                return Task.CompletedTask;
            }

            Domain.Entities.Jogador goleiro = await _copaDbContext.Jogador.FirstAsync(x => x.Id == evento.IdGoleiro);

            if (evento.DescricaoEvento == TipoEventoEnum.GolMarcado.ToString())
            {
                await RemoverGolMarcado(jogador);
                await RemoverGolSofrido(goleiro);
            }
            else if (evento.DescricaoEvento == TipoEventoEnum.GolContra.ToString())
                await RemoverGolSofrido(goleiro);

            _copaDbContext.EventosPartida.Remove(evento);

            return Task.CompletedTask;
        }

        private async Task RemoverCartaoVermelho(Domain.Entities.Jogador jogador)
        {
            jogador.CartoesAmarelos -= 1;
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task RemoverCartaoAmarelo(Domain.Entities.Jogador jogador)
        {
            jogador.CartoesAmarelos -= 1;
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task RemoverGolSofrido(Domain.Entities.Jogador goleiro)
        {
            goleiro.GolsSofridos -= 1;
            _copaDbContext.Jogador.Update(goleiro);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task RemoverGolMarcado(Domain.Entities.Jogador jogador)
        {
            jogador.GolsMarcados -= 1;
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddCartaoVermelho(int idJogador)
        {
            var jogador = await _copaDbContext.Jogador.FirstAsync(x => x.Id == idJogador);
            jogador.CartoesVemelhos += 1;
            jogador.Suspenso = true;

            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddCartaoAmarelo(int idJogador)
        {
            var jogador = await _copaDbContext.Jogador.FirstAsync(x => x.Id == idJogador);
            jogador.CartoesAmarelos += 1;

            if (jogador.CartoesAmarelos >= 3)
            {
                jogador.Suspenso = true;
                jogador.CartoesAmarelos = 0;
            }

            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddGolSofrido(int idGoleiro)
        {
            var goleiro = await _copaDbContext.Jogador.FirstAsync(x => x.Id == idGoleiro);
            goleiro.GolsSofridos += 1;
            _copaDbContext.Jogador.Update(goleiro);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task AddGolMarcado(int idJogador)
        {
            var jogador = await _copaDbContext.Jogador.FirstAsync(x => x.Id == idJogador);
            jogador.GolsMarcados += 1;
            _copaDbContext.Jogador.Update(jogador);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task SalvarScoreVisitante(PartidaModel partida, List<EventosPartida> cartoes)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdVisitante);
            time.GolsFeitos += partida.GolsVisitante;
            time.GolsSofridos += partida.GolsMandante;
            time.SaldoGols += partida.GolsVisitante - partida.GolsMandante;

            var cartoesAmarelos = cartoes.Where(x => x.DescricaoEvento == TipoEventoEnum.CartaoAmarelo.ToString()
                                                  && x.IdTime == partida.IdVisitante)
                                          .Count();

            var cartoesVermelhos = cartoes.Where(x => x.DescricaoEvento == TipoEventoEnum.CartaoVermelho.ToString()
                                                   && x.IdTime == partida.IdVisitante)
                                          .Count();

            time.CartoesAmarelos += cartoesAmarelos;
            time.CartoesVermelhos += cartoesVermelhos;

            _copaDbContext.Time.Update(time);
            await _copaDbContext.SaveChangesAsync();
        }

        private async Task SalvarScoreMandante(PartidaModel partida, List<EventosPartida> cartoes)
        {
            var time = await _copaDbContext.Time.FirstAsync(x => x.Id == partida.IdMandante);
            time.GolsFeitos += partida.GolsMandante;
            time.GolsSofridos += partida.GolsVisitante;
            time.SaldoGols += partida.GolsMandante - partida.GolsVisitante;

            var cartoesAmarelos = cartoes.Where(x => x.DescricaoEvento == TipoEventoEnum.CartaoAmarelo.ToString()
                                                  && x.IdTime == partida.IdMandante)
                                         .Count();

            var cartoesVermelhos = cartoes.Where(x => x.DescricaoEvento == TipoEventoEnum.CartaoVermelho.ToString()
                                                   && x.IdTime == partida.IdMandante)
                                          .Count();

            time.CartoesAmarelos += cartoesAmarelos;
            time.CartoesVermelhos += cartoesVermelhos;

            _copaDbContext.Time.Update(time);
            await _copaDbContext.SaveChangesAsync();
        }
    }
}
