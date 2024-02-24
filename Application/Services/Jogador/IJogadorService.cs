using Application.Services.Jogador.Model;

namespace Application.Services.Jogador
{
    public interface IJogadorService
    {
        Task<Task> CriaJogador(List<JogadorModel> jogadores);
        Task<List<JogadorViewModel>> BuscarJogadores(int idTime);
        Task<List<JogadorViewModel>> BuscarArtilheiros();
        Task<List<JogadorViewModel>> BuscarMelhoresGoleiros();
    }
}
