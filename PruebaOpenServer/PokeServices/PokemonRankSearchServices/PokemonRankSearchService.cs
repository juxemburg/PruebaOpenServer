using PokeServices.Exceptions;
using PokeServices.FactoryServices;
using PokeServices.PokedexServices;
using PokeServices.ViewModels;
using StateSearchEngine.Interfaces;
using StateSearchEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeServices.PokemonRankSearchServices
{
    public class PokemonRankSearchService
    {
        private readonly PokemonRankFactoryService _factoryService;
        private readonly PokedexProfilerService _profilerService;
        public PokemonRankSearchService(PokemonRankFactoryService factoryService,
            PokedexProfilerService profilerService)
        {
            _factoryService = factoryService;
            _profilerService = profilerService;
        }

        public async Task<ArenaResultsViewModel> RandomArenaResultsAsync(int pkmnCount)
            => await FullArenaSearchAsync(_profilerService.GetShuffledPokemonList(pkmnCount));

        /// <summary>
        /// Función que retorna los resultados básicos de la arena pokémon
        /// </summary>
        /// <param name="pkmnNames">Listado de los nombres de pokémon</param>
        /// <returns>un objeto de tipo PokemonRankSearchResultsViewModel, con el número de movimientos
        /// y el tiempo total de la búsqueda
        /// </returns>
        public Task<PokemonRankSearchResultsViewModel> DefaultArenaSearchAsync(List<string> pkmnNames)
            => Task.Run(() => ArenaSearch(pkmnNames,
                (searchResults, elapsedTime, id1, id2) => new PokemonRankSearchResultsViewModel()
                {
                    MovementCount = searchResults.Count - 1,
                    SearchElapsedTime = $"{elapsedTime}ms"
                }));

        public Task<ArenaResultsViewModel> FullArenaSearchAsync(List<string> pkmnNames)
            => Task.Run(() => ArenaSearch(pkmnNames,
                (searchResults, elapsedTime, initialId, goalId) => 
                    _profilerService.GetArenaResultsViewModel(elapsedTime,
                        searchResults.Select(result => ((PokemonRankViewModel)result).Battle).ToList(), 
                        initialId, goalId)));

        /// <summary>
        /// Funcion que realiza la búsqueda de los resultados de la arena pokémon
        /// </summary>
        /// <typeparam name="T">Tipo del formato del resultado obtenido</typeparam>
        /// <param name="pkmnNames">Listado de los nombres de pokémon</param>
        /// <param name="mapíngFunc">Función que mapea el resutlado de la búsqueda al formato especificado</param>
        /// <returns></returns>
        private T ArenaSearch<T>(List<string> pkmnNames, 
            Func<Queue<ISearchable<string>>, double, string, string, T> mapíngFunc)
        {
            if (pkmnNames.Count <= 1)
            {
                throw new OperationFailedException("No hay pokémones suficientes para iniciar el torneo. 🤔",
                        OperationErrorStatus.MalformedInput);
            }

            var initialState = _factoryService.CreateViewModel(pkmnNames, true);
            var goalState = _factoryService.CreateViewModel(pkmnNames);

            var goalStatePkmDict = goalState.PkmnPositions;
            var initialStatePkmDict = initialState.PkmnPositions;

            foreach (var key in initialStatePkmDict.Keys)
            {
                var distance = initialStatePkmDict[key].Item1 - goalStatePkmDict[key].Item1;

                // Si la distancia, menos 2, es positiva
                // es imposible que ese estado sea un candidato
                // por tanto, la búsqueda no tendrá un resultado
                if (distance - 2 > 0)
                {
                    throw new OperationFailedException("¡Vaya! Parece que los resultados entregados no son posibles, de acuerdo a las reglas del torneo. 🤔",
                        OperationErrorStatus.MalformedInput);
                }
            }

            var searchEngine = new AStarSearchEngine<string>(initialState, goalState, (pkCollection) =>
            {
                var pkDict = (pkCollection as PokemonRankViewModel).PkmnPositions;
                double heuristicValue = 0;
                foreach (var key in pkDict.Keys)
                {
                    // Se obtiene la distancia entre los pokémon
                    var distance = pkDict[key].Item1 - goalStatePkmDict[key].Item1;

                    // Si la distancia, menos 2, es positiva
                    // es imposible que ese estado sea un candidato
                    // por tanto se marca para no ser incluído en la búsqueda
                    if (distance - 2 > 0)
                    {
                        return double.MinValue;
                    }


                    // Si la distancia es mayor que 0, y el pokémon no puede pelear, 
                    // se penaliza el valor de la heurística por 1000
                    // de lo contrario se retorna la distancia obtenida / 2
                    heuristicValue += Math.Abs(distance) != 0 && !pkDict[key].Item2
                    ? 1000
                    : (Math.Abs(distance) / 2.0);
                }
                return heuristicValue;
            }, false);
            var searchResult = searchEngine.ShortestPathSearch();

            if (searchResult.Count == 0)
            {
                throw new OperationFailedException("¡Vaya! Parece que los resultados entregados no son posibles, de acuerdo a las reglas del torneo. 🤔",
                    OperationErrorStatus.MalformedInput);
            }

            return mapíngFunc(searchResult, searchEngine.ElapsedSearchTime, initialState.Id, goalState.Id);
        }
    }
}

