using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface ICopaDbContext
    {
        DbSet<Time> Time { get; set; }
        DbSet<Partida> Partida { get; set; }
        DbSet<Jogador> Jogador { get; set; }
        DbSet<EventosPartida> EventosPartida { get; set; }
        Task<int> SaveChangesAsync();
    }
}
