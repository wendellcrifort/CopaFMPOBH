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

        public async Task<List<TimeViewModel>> BuscarClassificacao()
        {

            var times = await _copaDbContext.Time
                                            .AsNoTracking()
                                            .OrderByDescending(o => o.Pontos)
                                            .ThenByDescending(o => o.Vitorias)
                                            .ThenByDescending(o => o.GolsFeitos)
                                            .ThenByDescending(o => o.SaldoGols)
                                            .ToListAsync();

            return _mapper.Map<List<TimeViewModel>>(times);
        }
    }
}
