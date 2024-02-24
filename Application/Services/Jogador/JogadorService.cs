using Application.Common.Interfaces;
using Application.Services.Jogador.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Jogador
{
    public class JogadorService : IJogadorService
    {
        private readonly IMapper _mapper;
        private readonly ICopaDbContext _copaDbContext;

        public JogadorService(IMapper mapper, ICopaDbContext copaDbContext)
        {
            _mapper = mapper;
            _copaDbContext = copaDbContext;
        }

        public async Task<Task> CriaJogador(List<JogadorModel> jogadores)
        {
            var listaJogadores = new List<Domain.Entities.Jogador>();
            foreach (var jogador in jogadores)            
                listaJogadores.Add(_mapper.Map<Domain.Entities.Jogador>(jogador));

            _copaDbContext.Jogador.AddRange(listaJogadores);
            await _copaDbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<List<JogadorViewModel>> BuscarJogadores(int idTime)
        {
            var jogadores = await _copaDbContext.Jogador
                                                .AsNoTracking()
                                                .Where(x => x.IdTime == idTime)
                                                .OrderBy(o => o.Nome)
                                                .ToListAsync();

            return _mapper.Map<List<JogadorViewModel>>(jogadores);
        }

        public async Task<List<JogadorViewModel>> BuscarArtilheiros() 
        {
            var jogadores = await _copaDbContext.Jogador
                                                .AsNoTracking()
                                                .Where(x => x.GolsMarcados > 0)
                                                .OrderByDescending(o => o.GolsMarcados)
                                                .OrderBy(o => o.Jogos)
                                                .ToListAsync();

            return _mapper.Map<List<JogadorViewModel>>(jogadores);
        }

        public async Task<List<JogadorViewModel>> BuscarMelhoresGoleiros()
        {
            var jogadores = await _copaDbContext.Jogador
                                                .AsNoTracking()
                                                .Where(x => x.EhGoleiro && x.Jogos > 0)
                                                .OrderBy(o => o.GolsSofridos)
                                                .ThenByDescending(o => o.Jogos)
                                                .ToListAsync();

            return _mapper.Map<List<JogadorViewModel>>(jogadores);
        }
    }
}
