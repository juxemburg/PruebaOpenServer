using System.Collections.Generic;
using System.Threading.Tasks;
using PokeServer.ControllerUtils;
using Microsoft.AspNetCore.Mvc;
using PokeServices.ArenaServices;

namespace PokeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonArenaController : Controller
    {
        private readonly PokemonArenaService _arenaService;

        public PokemonArenaController(PokemonArenaService arenaService)
        {
            _arenaService = arenaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResources()
            => await this.Get(async () => await _arenaService.GetAllArenaResultsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResource(int id)
            => await this.Get(async () => await _arenaService.GetArenaResultsAsync(id));

        [HttpPost]
        public async Task<IActionResult> AddResource([FromBody] List<string> pkmnNames)
            => await this.Post(ModelState, async () => await _arenaService.CreateArenaAsync(pkmnNames));

        [HttpPost("Random")]
        public async Task<IActionResult> AddResourceRandom([FromBody] int pkmnCount)
            => await this.Post(ModelState, async () => await _arenaService.CreateRandomArenaAsync(pkmnCount));
    }
}