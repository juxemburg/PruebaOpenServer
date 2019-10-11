using PokeServices.Exceptions;
using PokeServices.PokedexServices;
using PokeServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokeServices.FactoryServices
{
    /// <summary>
    /// Clase encargada de crear el listado de pokémons 
    /// con su respectivo código de Pokédex
    /// </summary>
    public class PokemonRankFactoryService
    {
        private readonly PokedexProfilerService _profilerService;

        private readonly Func<string, PokemonRankItem> _dexerFunc;

        public PokemonRankFactoryService(PokedexProfilerService profilerService)
        {
            _profilerService = profilerService;
            _dexerFunc = (name) =>
            {
                int dexNum = _profilerService.GetPokedexNumber(name);
                if (dexNum == -1)
                {
                    throw new OperationFailedException($"El nombre del pokémon '{name}' no existe. 😞",
                        OperationErrorStatus.MalformedInput);
                }
                return new PokemonRankItem()
                {
                    DexNumber = dexNum,
                    PkmnName = name
                };
            };
        }

        /// <summary>
        /// Función que retorna el modelo de búsqueda de peleas de pokémon
        /// </summary>
        /// <param name="pkmnNames">Listado de nombres de pokémon</param>
        /// <param name="dexNumSort">Indica si la lista se debe ordenar por número de pokédex</param>
        /// <returns></returns>
        public PokemonRankViewModel CreateViewModel(List<string> pkmnNames, bool dexNumSort = false)
        {
            var dict = new Dictionary<string, int>();
            var pkmns = new List<PokemonRankItem>();
            //var pkmns = dexNumSort ? pkmnNames.Select(_dexerFunc) : pkmnNames.Select(_dexerFunc).OrderBy(p => p.DexNumber);

            foreach (var name in pkmnNames)
            {
                if (dict.ContainsKey(name))
                {
                    throw new OperationFailedException($"¡Vaya! Parece que {name} intenta competir más de una vez. Esto no está permitido. 😒",
                        OperationErrorStatus.MalformedInput);
                }
                dict.Add(name, 1);
                pkmns.Add(_dexerFunc(name));
            }
            if(dexNumSort)
            {
                pkmns.Sort((a, b) => a.DexNumber.CompareTo(b.DexNumber));
            }
            return new PokemonRankViewModel(pkmns.ToList(), 0, "");
        }

    }
}
