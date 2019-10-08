using StateSearchEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatSearchEngineConsoleClient.DummyClasses
{
    public class DummyPkmnCollection : ISearchable<string>
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public double Score { get; set; }

        public bool Equals(string other)
        {
            return Id.Equals(other);
        }

        //<DexId, PkmnName, RemainingBattles>
        private readonly List<Tuple<int, string, int>> _pkmnList;

        public DummyPkmnCollection(List<Tuple<int, string, int>> pkmnList, double score, string parentId)
        {
            _pkmnList = pkmnList;
            Score = score;
            ParentId = parentId;

            Id = _pkmnList.Aggregate("", (acc, current) =>
                string.IsNullOrWhiteSpace(acc) ? $"{current.Item1}" : $"{acc}-{current.Item1}");
        }

        public IEnumerable<ISearchable<string>> Extend()
        {
            var childs = new List<ISearchable<string>>();
            for (int i = _pkmnList.Count - 1; i >= 1; i--)
            {
                if (_pkmnList[i].Item3 < 1)
                {
                    continue;
                }

                var nthClone = _pkmnList.Select(item => new Tuple<int, string, int>(item.Item1, item.Item2, item.Item3)).ToList();
                var aux = nthClone[i - 1];
                nthClone[i] = new Tuple<int, string, int>(nthClone[i].Item1, nthClone[i].Item2, nthClone[i].Item3 - 1);
                nthClone[i - 1] = nthClone[i];
                nthClone[i] = aux;
                childs.Add(new DummyPkmnCollection(nthClone, Score + 1, Id));
            }
            return childs;
        }
    }
}
