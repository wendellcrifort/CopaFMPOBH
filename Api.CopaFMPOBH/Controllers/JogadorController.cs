using Application.Services.Jogador;
using Application.Services.Jogador.Model;
using Application.Services.Time;
using Application.Services.Time.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.CopaFMPOBH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogadorController : ControllerBase
    {
        private readonly IJogadorService _jogadorService;
        private readonly ITimeService _timesService;

        public JogadorController(IJogadorService jogadorService, ITimeService timeService)
        {
            _jogadorService = jogadorService;
            _timesService = timeService;
        }

        [HttpPost("[Action]")]
        public async Task<ActionResult<int>> Jogador(List<JogadorModel> jogadores)
        {
            var result = await _jogadorService.CriaJogador(jogadores);
            return Ok(result);
        }

        [HttpGet("[Action]/{idTime}")]
        public async Task<ActionResult<List<JogadorViewModel>>> BuscarJogadores(int idTime)
        {
            var jogadores = await _jogadorService.BuscarJogadores(idTime);
            return Ok(jogadores);
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult<List<TimeViewModel>>> BuscarTimes()
        {
            var times = await _timesService.BuscarTimes();
            return Ok(times);
        }
    }
}
