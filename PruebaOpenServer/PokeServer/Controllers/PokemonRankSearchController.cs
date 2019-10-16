using System.Collections.Generic;
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

        [HttpGet("RandomArenaResults")]
        public async Task<IActionResult> GetRandomArenaResults([FromQuery] int pkmnCount = 50)
            => await this.Get(async () => await _rankSearchService.RandomArenaResultsAsync(pkmnCount));

        [HttpPost("ArenaResults")]
        public async Task<IActionResult> ArenaResults([FromBody] List<string> pkmnNames)
            => await this.Post(ModelState, async () => await _rankSearchService.FullArenaSearchAsync(pkmnNames));

        // POST: api/PokemonRankSearch
        [HttpPost]
        public async Task<IActionResult> PkmnRankSearch([FromBody] List<string> pkmnNames)
            => await this.Post(ModelState, async () => await _rankSearchService.DefaultArenaSearchAsync(pkmnNames));


    }
}