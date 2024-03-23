namespace Application.Services.Partida.Model
{
    public class PartidasHomeViewModel
    {
        public List<PartidaViewModel> PartidasAoVivo { get; set; } = new List<PartidaViewModel>();
        public PartidaViewModel ProximaPartida { get; set; } = new PartidaViewModel();
        public List<PartidaViewModel> PartidasEncerradas { get; set; } = new List<PartidaViewModel>();
    }
}
