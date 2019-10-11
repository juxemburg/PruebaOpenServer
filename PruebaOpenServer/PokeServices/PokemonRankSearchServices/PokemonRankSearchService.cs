using PokeServices.Exceptions;
using PokeServices.FactoryServices;
using PokeServices.ViewModels;
using StateSearchEngine.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeServices.PokemonRankSearchServices
{
    public class PokemonRankSearchService
    {
        private readonly PokemonRankFactoryService _factoryService;
        public PokemonRankSearchService(PokemonRankFactoryService factoryService)
        {
            _factoryService = factoryService;
        }

        public Task<PokemonRankSearchResultsViewModel> RankSearchAsync(List<string> pkmnNames)
        {
            return Task.Run(() =>
            {
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


                        // Si la distancia es 0, y el pokémon no puede pelear, 
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

                return new PokemonRankSearchResultsViewModel()
                {
                    MovementCount = searchResult.Count - 1,
                    SearchElapsedTime = $"{searchEngine.ElapsedSearchTime}ms"
                };
            });

        }
    }
}

