using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    public class PlacarHub : Hub
    {
        public async Task AtualizarPlacar(int idPartida, int golsMandante, int golsVisitante)
        {
            await Clients.All.SendAsync("ReceberAtualizacaoPlacar", idPartida, golsMandante, golsVisitante);
        }
    }
}
