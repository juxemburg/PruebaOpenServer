using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeServer.ControllerUtils;
using PokeServices.PokedexServices;
using PokeServices.PokemonRankSearchServices;

namespace PokeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonRankSearchController : Controller
    {
        private readonly PokemonRankSearchService _rankSearchService;
        private readonly PokedexProfilerService _profilerService;
        public PokemonRankSearchController(PokemonRankSearchService rankSearchService,
            PokedexProfilerService profilerService)
        {
            _rankSearchService = rankSearchService;
            _profilerService = profilerService;
        }

        [HttpGet("ShuffledPokemonList")]
        public async Task<IActionResult> GetShuffledPokemonList([FromQuery] int pkmnCount = 50)
            => await this.Get(async () => await _profilerService.GetShuffledPokemonListAsync(pkmnCount));

        // POST: api/PokemonRankSearch
        [HttpPost]
        public async Task<IActionResult> PkmnRankSearch([FromBody] List<string> pkmnNames)
            => await this.Post(ModelState, async () => await _rankSearchService.RankSearchAsync(pkmnNames));


    }
}