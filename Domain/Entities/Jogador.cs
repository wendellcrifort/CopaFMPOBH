using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Jogador")]
    public class Jogador
    {        
        [Column("IdJogador")]
        public int Id { get; set; }

        [Column("IdTime")]
        public int IdTime { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("Igreja")]
        public string Igreja { get; set; }

        [Column("Numero")]
        public int Numero { get; set; }

        [Column("Idade")]
        public int Idade { get; set; }

        [Column("GolsMarcados")]
        public int? GolsMarcados { get; set; } = 0;

        [Column("GolsSofridos")]
        public int? GolsSofridos { get; set; } = 0;

        [Column("EhGoleiro")]
        public bool EhGoleiro { get; set; } = false;

        [Column("Jogos")]
        public int? Jogos { get; set; } = 0;

        [Column("CartoesAmarelos")]
        public int? CartoesAmarelos { get; set; } = 0;

        [Column("CartoesVemelhos")]
        public int? CartoesVemelhos { get; set; } = 0;

        [Column("Suspenso")]
        public bool? Suspenso { get; set; } = false;

        [ForeignKey("IdTime")]
        public virtual Time Time { get; set; } = new Time();
    }
}
