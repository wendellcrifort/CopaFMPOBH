using Application.Common.Enum;
using Application.Services.Partida.Model;

namespace Application.Services.Partida
{
    public interface IPartidaService
    {
        Task<int> CriarPartida(PartidaModel partida);
        Task<List<PartidaViewModel>> BuscarPartidas(string? data);
        Task<PartidaViewModel> BuscarPartidaEmAndamento(int idPartida);
        Task FinalizarPartida(PartidaModel partida);
        Task<EventoPlacarViewModel> RegistrarEventoPartida(int idPartida, int idJogador, TipoEventoEnum evento, int? idGoleiro);
        Task<EventoPlacarViewModel> RemoverEventoPartida(int idEvento);
        Task<List<EventosPartidaViewModel>> BuscarEventosPartida(int idPartida);
        Task<int> IniciarPartida(int idPartida);
    }
}
