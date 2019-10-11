using StateSearchEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PokeServices.ViewModels
{
    public class PokemonRankViewModel : ISearchable<string>
    {
        public bool Equals(string other)
        {
            return Id.Equals(other);
        }

        //<DexId, PkmnName, RemainingBattles>
        private readonly List<PokemonRankItem> _pkmnList;

        //<DexId, ListPosition>
        public readonly Dictionary<int, Tuple<int, bool>> PkmnPositions;

        public PokemonRankViewModel(List<PokemonRankItem> pkmnList, double depth, string parentId)
            : base(depth)
        {
            _pkmnList = pkmnList;
            ParentId = parentId;
            PkmnPositions = new Dictionary<int, Tuple<int, bool>>();

            var pos = 0;
            foreach (var pkmn in _pkmnList)
            {
                Id += $"-{pkmn.DexNumber}";
                PkmnPositions.Add(pkmn.DexNumber, new Tuple<int, bool>(pos, pkmn.CanFight));
                pos++;
            }
        }

        public PokemonRankViewModel(List<PokemonRankItem> pkmnList, double depth, string parentId, 
            string beforeId, string afterId)
            : base(depth)
        {
            _pkmnList = pkmnList;
            ParentId = parentId;
            PkmnPositions = new Dictionary<int, Tuple<int, bool>>();
            
            Id = Regex.Replace(parentId, $"(?<=-|^)({beforeId})(?=$|-)", afterId);
            var pos = 0;
            foreach (var pkmn in _pkmnList)
            {
                //Id = string.IsNullOrWhiteSpace(Id) ? $"${pkmn.DexNumber}" : $"{Id}-{pkmn.DexNumber}";
                PkmnPositions.Add(pkmn.DexNumber, new Tuple<int, bool>(pos, pkmn.CanFight));
                pos++;
            }
        }

        public override IEnumerable<ISearchable<string>> Extend()
        {
            //TODO: Optimizar el proceso de creación de hijitos
            //TODO: revisar si es posible evitar crear hijos cuando 
            //su puntaje inicial es mayor al puntaje del candidato encontrado.
            var childs = new List<ISearchable<string>>();
            for (int i = _pkmnList.Count - 1; i >= 1; i--)
            {
                if (!_pkmnList[i].CanFight)
                {
                    continue;
                }

                var nthClone = _pkmnList.Select(item => item.Clone()).ToList();

                var beforeId = $"{nthClone[i - 1].DexNumber}-{nthClone[i].DexNumber}";
                var aux = nthClone[i - 1];
                nthClone[i].Fight();
                nthClone[i - 1] = nthClone[i];
                nthClone[i] = aux;
                var afterId = $"{nthClone[i - 1].DexNumber}-{nthClone[i].DexNumber}";
                childs.Add(new PokemonRankViewModel(nthClone, Depth + 1, Id));
            }
            return childs;
        }
    }
}
