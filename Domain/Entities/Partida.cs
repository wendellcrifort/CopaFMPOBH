using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Partida")]
    public class Partida
    {
        [Column("IdPartida")]
        public int Id { get; set; }

        [Column("DataPartida")]
        public string DataPartida { get; set; }

        [Column("HoraPartida")]
        public string HoraPartida { get; set; }

        [Column("Rodada")]
        public int Rodada { get; set; }

        [Column("IdTimeMandante")]
        public int IdTimeMandante { get; set; }

        [Column("GolsTimeMandante")]
        public int GolsTimeMandante { get; set; }

        [Column("PontosTimeMandante")]
        public int PontosTimeMandante { get; set; }

        [Column("IdTimeVisitante")]
        public int IdTimeVisitante { get; set; }

        [Column("GolsTimeVisitante")]
        public int GolsTimeVisitante { get; set; }

        [Column("PontosTimeVisitante")]
        public int PontosTimeVisitante { get; set; }
    }
}
