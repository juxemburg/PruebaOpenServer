using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeServer.ControllerUtils;
using PokeServices.PokedexServices;
using PokeServices.PokemonServices;

namespace PokeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly PokemonService _pokemonService;
        private readonly PokedexProfilerService _profilerService;
        public PokemonController(PokemonService pokemonService, PokedexProfilerService profilerService)
        {
            _pokemonService = pokemonService;
            _profilerService = profilerService;
        }

        [HttpGet("shortinfo")]
        public async Task<IActionResult> AllPokemonShortInfo()
            => await this.Get(async () => await _profilerService.GetPokemonShortInfoViewModelsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPokemon(int id)
            => await this.Get(async () => await _pokemonService.GetPokemonInfoAsync(id));

        
    }
}