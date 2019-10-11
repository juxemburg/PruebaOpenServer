using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeApp.ControllerUtils;
using PokeServices.PokemonRankSearchServices;

namespace PokeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonRankSearchController : Controller
    {
        private readonly PokemonRankSearchService _rankSearchService;
        public PokemonRankSearchController(PokemonRankSearchService rankSearchService)
        {
            _rankSearchService = rankSearchService;
        }

        // POST: api/PokemonRankSearch
        [HttpPost]
        public async Task<IActionResult> PkmnRankSearch([FromBody] List<string> pkmnNames)
            => await this.Post(ModelState, async () => await _rankSearchService.RankSearchAsync(pkmnNames));


    }
}
