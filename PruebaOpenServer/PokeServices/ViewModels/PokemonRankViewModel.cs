using StateSearchEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public readonly Dictionary<int, int> PkmnPositions;

        public PokemonRankViewModel(List<PokemonRankItem> pkmnList, double depth, string parentId)
            : base(depth)
        {
            _pkmnList = pkmnList;
            ParentId = parentId;
            PkmnPositions = new Dictionary<int, int>();

            var pos = 0;
            foreach (var pkmn in _pkmnList)
            {
                Id = string.IsNullOrWhiteSpace(Id) ? $"${pkmn.DexNumber}" : $"{Id}-{pkmn.DexNumber}";
                PkmnPositions.Add(pkmn.DexNumber, pos);
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
                var aux = nthClone[i - 1];
                nthClone[i].Fight();
                nthClone[i - 1] = nthClone[i];
                nthClone[i] = aux;
                childs.Add(new PokemonRankViewModel(nthClone, Score + 1, Id));
            }
            return childs;
        }
    }
}
