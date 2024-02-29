namespace Application.Services.Partida.Model
{
    public class PartidaModel
    {        
        public int? IdPartida { get; set; }
        public int IdMandante { get; set; }        
        public int IdVisitante { get; set; }    
        public string? Data { get; set; }
        public string? Hora { get; set; }
        public int? GolsMandante { get; set; }
        public int? GolsVisitante { get; set; }
    }
}
