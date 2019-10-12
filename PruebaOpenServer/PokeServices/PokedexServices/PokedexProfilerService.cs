using PokeApiNet;
using PokeApiNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeServices.PokedexServices
{
    public class PokedexProfilerService
    {
        private readonly PokeApiClient _pokeClient = new PokeApiClient();

        private Dictionary<string, int> _pokedexNameIndex = new Dictionary<string, int>();
        private Dictionary<int, string> _pokedexNumIndex = new Dictionary<int, string>();

        public PokedexProfilerService()
        {

        }

        public async Task InitAsync()
        {
            var pokedex = await _pokeClient.GetResourceAsync<Pokedex>(1);
            _pokedexNameIndex = pokedex.PokemonEntries.ToDictionary(entry => entry.PokemonSpecies.Name, entry => entry.EntryNumber);
            _pokedexNumIndex = pokedex.PokemonEntries.ToDictionary(entry => entry.EntryNumber, entry => entry.PokemonSpecies.Name);
        }

        /// <summary>
        /// Método que retorna el código de la pókedex dado el nombre de un pokémon
        /// </summary>
        /// <param name="pkmnName">Nombre del pokémon</param>
        /// <returns>Código de la pokédex, en caso de que el pokémon no exista se retorna -1</returns>
        public int GetPokedexNumber(string pkmnName)
            => _pokedexNameIndex.ContainsKey(pkmnName.ToLower()) ? _pokedexNameIndex[pkmnName.ToLower()] : -1;


        public Task<List<string>> GetShuffledPokemonListAsync(int pkmnCount = 50)
            => Task.Run(() => GetShuffledPokemonList(pkmnCount));

        public List<string> GetShuffledPokemonList(int pkmnCount = 50)
        {
            var positionDict = Enumerable.Range(1, pkmnCount)
                .Where(dexNum => _pokedexNumIndex.ContainsKey(dexNum))
                .Select(dexNum => _pokedexNumIndex[dexNum])
                .ToList();

            positionDict.Shuffle();
            return positionDict;

        }

    }
}
