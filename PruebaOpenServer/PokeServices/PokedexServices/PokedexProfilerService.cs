using PokeApiNet;
using PokeApiNet.Models;
using PokeServices.ViewModels;
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

        public PokedexProfilerService() { }

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

        /// <summary>
        /// Retorna una lista con los nombres de pokemon desordenados, 
        /// para ser usados en una nueva búsqueda
        /// </summary>
        /// <param name="pkmnCount"></param>
        /// <returns></returns>
        public List<string> GetShuffledPokemonList(int pkmnCount = 50)
        {
            var positionDict = Enumerable.Range(1, pkmnCount)
                .Where(dexNum => _pokedexNumIndex.ContainsKey(dexNum))
                .Select(dexNum => _pokedexNumIndex[dexNum])
                .ToList();

            positionDict.Shuffle();
            return positionDict;
        }

        /// <summary>
        /// Retorna un listado con el resultado de la arena pokemon
        /// </summary>
        /// <param name="fightCount">El número de batallas </param>
        /// <param name="battleRecords"></param>
        /// <returns></returns>
        public ArenaResultsViewModel GetArenaResultsViewModel(double elapsedTime,
            List<string> battleRecords, string initialStateId, string finalStateId)
        {
            return new ArenaResultsViewModel()
            {
                InitialArenaPosition = initialStateId.Split("-")
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => getPokemonShortInfo(Convert.ToInt32(id)))
                    .ToList(),
                FinalArenaPosition = finalStateId.Split("-")
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => getPokemonShortInfo(Convert.ToInt32(id)))
                    .ToList(),
                FightCount = battleRecords.Count - 1,
                ArenaElapsedTime = elapsedTime,
                BattleRecords = battleRecords
                .Where(record => !string.IsNullOrWhiteSpace(record))
                .Select((record, index) =>
                {
                    var idList = record.Split("-").Where(item => !string.IsNullOrWhiteSpace(item));
                    var challengerId = Convert.ToInt32(idList.ElementAt(0));
                    var challengedId = Convert.ToInt32(idList.ElementAt(1));

                    return new PokemonArenaBattleViewModel()
                    {
                        Order = (battleRecords.Count - 1) - index,
                        ChallengerPokemon = getPokemonShortInfo(challengerId),
                        ChallengedPokemon = getPokemonShortInfo(challengedId)
                    };
                }).OrderBy(item => item.Order).ToList()
            };

        }

        private PokemonShortInfoViewModel getPokemonShortInfo(int dexNum)
         => new PokemonShortInfoViewModel()
         {
             DexNum = dexNum,
             Name = _pokedexNumIndex[dexNum]
         };
    }
}
