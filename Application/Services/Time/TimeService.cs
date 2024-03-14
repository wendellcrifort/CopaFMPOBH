using Application.Common.Interfaces;
using Application.Services.Time.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Time
{
    public class TimeService : ITimeService
    {
        private readonly IMapper _mapper;
        private readonly ICopaDbContext _copaDbContext;
        public TimeService(ICopaDbContext copaDbContext, IMapper mapper) 
        {
            _copaDbContext = copaDbContext;
            _mapper = mapper;
        }
        public async Task<List<TimeViewModel>> BuscarTimes()
        {
            var times = await _copaDbContext.Time
                                            .AsNoTracking()
                                            .OrderBy(o => o.Nome)
                                            .ToListAsync();

            return _mapper.Map<List<TimeViewModel>>(times);
        }

        public async Task<ClassificacaoViewModel> BuscarClassificacao()
        {
            var times = await _copaDbContext.Time
                                            .AsNoTracking()
                                            .ToListAsync();

            var classificacao = new ClassificacaoViewModel();
            classificacao.GrupoA = _mapper.Map<List<TimeViewModel>>(
                                                times.Where(x => x.Grupo == "A")
                                                     .OrderByDescending(o => o.Pontos)
                                                     .ThenByDescending(o => o.Vitorias)
                                                     .ThenByDescending(o => o.GolsFeitos)
                                                     .ThenByDescending(o => o.SaldoGols)
                                                     .ThenBy(o => o.Nome)
                                                     .ToList());

            classificacao.GrupoB = _mapper.Map<List<TimeViewModel>>(
                                                times.Where(x => x.Grupo == "B")
                                                     .OrderByDescending(o => o.Pontos)
                                                     .ThenByDescending(o => o.Vitorias)
                                                     .ThenByDescending(o => o.GolsFeitos)
                                                     .ThenByDescending(o => o.SaldoGols)
                                                     .ThenBy(o => o.Nome)
                                                     .ToList());

            return classificacao;
        }
    }
}
