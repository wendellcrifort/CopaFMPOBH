using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("EventosPartida")]
    public class EventosPartida
    {
        [Column("IdEvento")]
        public int Id { get; set; }

        [Column("IdPartida")]
        public int IdPartida { get; set; }

        [Column("IdJogador")]
        public int IdJogador { get; set; }

        [Column("IdTime")]
        public int IdTime { get; set; }

        [Column("IdGoleiro")]
        public int? IdGoleiro { get; set; }

        [Column("DescricaoEvento")]
        public string? DescricaoEvento { get; set; }
                
        [ForeignKey("IdPartida")]
        public virtual Partida? Partida { get; set; }
                
        [ForeignKey("IdJogador")]
        public virtual Jogador? Jogador { get; set; }
    }
}
