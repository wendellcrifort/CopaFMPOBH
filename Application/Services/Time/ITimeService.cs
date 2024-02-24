using Application.Services.Time.Model;

namespace Application.Services.Time
{
    public interface ITimeService
    {
        Task<List<TimeViewModel>> BuscarTimes();
        Task<List<TimeViewModel>> BuscarClassificacao();
    }
}
