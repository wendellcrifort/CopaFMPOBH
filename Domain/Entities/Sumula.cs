using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Sumula")]
    public class Sumula
    {
        [Column("IdSumula")]
        public int Id { get; set; }

        [Column("IdPartida")]
        public int IdPartida { get; set; }

        [Column("Sumula")]
        public string Arquivo { get; set; }

        [ForeignKey("IdPartida")]
        public virtual Partida? Partida { get; set; }
    }
}
