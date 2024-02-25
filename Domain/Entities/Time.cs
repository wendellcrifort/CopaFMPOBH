using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{    
    [Table("Time")]
    public class Time
    {        
        [Column("IdTime")]
        public int Id { get; set; }

        [Column("Grupo")]
        public string? Grupo { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("Pontos")]
        public int? Pontos { get; set; }

        [Column("Vitorias")]
        public int? Vitorias { get; set; }

        [Column("Empates")]
        public int? Empates { get; set; }

        [Column("Derrotas")]
        public int? Derrotas { get; set; }

        [Column("GolsFeitos")]
        public int? GolsFeitos { get; set; }

        [Column("GolsSofridos")]
        public int? GolsSofridos { get; set; }

        [Column("SaldoGols")]
        public int? SaldoGols { get; set; }

        [Column("CartoesAmarelos")]
        public int? CartoesAmarelos { get; set; }

        [Column("CartoesVermelhos")]
        public int? CartoesVermelhos { get; set; }

        [Column("Escudo")]
        public string? Escudo { get; set; }
    }
}
