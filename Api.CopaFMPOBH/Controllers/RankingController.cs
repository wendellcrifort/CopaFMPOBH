using Application.Services.Jogador;
using Application.Services.Jogador.Model;
using Application.Services.Time;
using Application.Services.Time.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.CopaFMPOBH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RankingController : ControllerBase
    {

        private readonly ITimeService _timeService;
        private readonly IJogadorService _jogadorService;

        public RankingController(ITimeService timeService, IJogadorService jogadorService)
        {
            _timeService = timeService;
            _jogadorService = jogadorService;
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult<TimeViewModel>> BuscarClassificacao()
        {
            var classificacao = await _timeService.BuscarClassificacao();
            return Ok(classificacao);
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult<List<JogadorViewModel>>> BuscarArtilheiros()
        {
            var artilheiros = await _jogadorService.BuscarArtilheiros();
            return Ok(artilheiros);
        }

        [HttpGet("[Action]")]
        public async Task<ActionResult<List<JogadorViewModel>>> BuscarMelhoresGoleiros()
        {
            var melhoresGoleiros = await _jogadorService.BuscarMelhoresGoleiros();
            return Ok(melhoresGoleiros);
        }
    }
}
