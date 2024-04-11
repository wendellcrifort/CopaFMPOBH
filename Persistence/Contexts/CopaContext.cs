using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class CopaContext : DbContext, ICopaDbContext
    {
        public CopaContext(DbContextOptions<CopaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Time> Time { get; set; }
        public virtual DbSet<Partida> Partida { get; set; }
        public virtual DbSet<Jogador> Jogador { get; set; }
        public virtual DbSet<EventosPartida> EventosPartida { get; set; }

        public virtual DbSet<Sumula> Sumula { get; set; }

        public virtual Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

    }
}
