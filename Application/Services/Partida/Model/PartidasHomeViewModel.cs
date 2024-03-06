namespace Application.Services.Partida.Model
{
    public class PartidasHomeViewModel
    {
        public PartidaViewModel PartidaAoVivo { get; set; } = new PartidaViewModel();
        public PartidaViewModel ProximaPartida { get; set; } = new PartidaViewModel();
        public List<PartidaViewModel> PartidasEncerradas { get; set; } = new List<PartidaViewModel>();
    }
}
