using PokeApiNet;
using PokeApiNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeServices.PokedexServices
{
    public class PokedexProfilerService
    {
        private readonly PokeApiClient _pokeClient = new PokeApiClient();

        private Dictionary<string, int> _pokedexIndex = new Dictionary<string, int>();

        public PokedexProfilerService()
        {

        }

        public async Task InitAsync()
        {
            var pokedex = await _pokeClient.GetResourceAsync<Pokedex>(1);
            _pokedexIndex = pokedex.PokemonEntries.ToDictionary(entry => entry.PokemonSpecies.Name, entry => entry.EntryNumber);
        }

        /// <summary>
        /// Método que retorna el código de la pókedex dado el nombre de un pokémon
        /// </summary>
        /// <param name="pkmnName">Nombre del pokémon</param>
        /// <returns>Código de la pokédex, en caso de que el pokémon no exista se retorna -1</returns>
        public int GetPokedexNumber(string pkmnName)
            => _pokedexIndex.ContainsKey(pkmnName) ? _pokedexIndex[pkmnName] : -1;

    }
}
