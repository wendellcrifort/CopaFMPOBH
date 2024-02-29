using Application.Common.Enum;
using Application.Services.Partida;
using Application.Services.Partida.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.CopaFMPOBH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly IPartidaService _partidaService;

        public PartidaController(IPartidaService partidaService)
        {
            _partidaService = partidaService;
        }

        [HttpPost("[Action]")]
        public async Task<ActionResult<int>> Partida(PartidaModel partida)
        {
            return Ok(await _partidaService.CriarPartida(partida));
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult<List<PartidaViewModel>>> BuscarPartidas(string? data)
        {
            var partidas = await _partidaService.BuscarPartidas(data);
            return Ok(partidas);
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult<PartidaViewModel>> BuscarPartidaEmAndamento(int idPartida)
        {
            var partida = await _partidaService.BuscarPartidaEmAndamento(idPartida);
            return Ok(partida);
        }

        [HttpGet("[Action]/{idPartida}")]
        public async Task<ActionResult<List<EventosPartidaViewModel>>> BuscarEventosPartidas(int idPartida)
        {
            var partidas = await _partidaService.BuscarEventosPartida(idPartida);
            return Ok(partidas);
        }

        [HttpPatch("[Action]")]
        public IActionResult FinalizarPartida(PartidaModel partida)
        {
            _partidaService.FinalizarPartida(partida);

            return Ok();
        }

        [HttpPost("[Action]")]
        public async Task<ActionResult<int>> RegistrarEventoPartida(int idPartida, int idJogador, TipoEventoEnum evento, int idGoleiro)
        {
            var idEvento = await _partidaService.RegistrarEventoPartida(idPartida, idJogador, evento, idGoleiro);
            return Ok(idEvento);
        }

        [HttpDelete("[Action]")]
        public IActionResult RemoverEventoPartida(int idEvento)
        {
            _partidaService.RemoverEventoPartida(idEvento);
            return Ok();
        }
    }
}
