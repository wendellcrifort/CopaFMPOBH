using Application.Services.Jogador.Model;

namespace Application.Services.Jogador
{
    public interface IJogadorService
    {
        Task<int> CriaJogador(List<JogadorModel> jogadores);
        Task<JogadorTimeViewModel> BuscarJogadores(int idTime);
        Task<List<JogadorViewModel>> BuscarArtilheiros();
        Task<List<JogadorViewModel>> BuscarMelhoresGoleiros();
    }
}
