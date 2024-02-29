namespace Application.Services.Partida.Model
{
    public class PartidaViewModel
    {
        public int IdPartida { get; set; }
        public string? DataPartida { get; set; }
        public string? HoraPartida { get; set; }
        public int IdTimeMandante { get; set; }
        public int GolsTimeMandante { get; set; }
        public int PontosTimeMandante { get; set; }
        public int IdTimeVisitante { get; set; }
        public int GolsTimeVisitante { get; set; }
        public int PontosTimeVisitante { get; set; }
        public int Rodada { get; set; }
    }
}
