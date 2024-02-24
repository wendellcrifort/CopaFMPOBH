using Application.Common.Enum;
using Application.Services.Partida.Model;

namespace Application.Services.Partida
{
    public interface IPartidaService
    {
        Task CriarPartida(PartidaModel partida);
        Task<List<PartidaViewModel>> BuscarPartidas(string data);
        Task FinalizarPartida(PartidaModel partida);
        Task<int> RegistrarEventoPartida(int idPartida, int idJogador, TipoEventoEnum evento, int idGoleiro);
        Task<Task> RemoverEventoPartida(int idEvento);
        Task<List<EventosPartidaViewModel>> BuscarEventosPartida(int idPartida);
    }
}
