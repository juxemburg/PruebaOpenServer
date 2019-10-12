using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StateSearchEngine.Interfaces
{
    public abstract class ISearchable<T> : IComparable<ISearchable<T>>
    {
        public T Id { get; set; }

        public T ParentId { get; set; }

        public double Score { get { return Depth + HeuristicValue; } }

        public double HeuristicValue { get; set; } = 0;

        public double Depth { get; private set; }

        public ISearchable(double depth)
        {
            Depth = depth;
        }

        public abstract IEnumerable<ISearchable<T>> Extend(Func<ISearchable<T>, double> heuristicFn, ISearchable<T> goalState);

        public int CompareTo(ISearchable<T> other)
        {
            return Score.CompareTo(other.Score);
        }

    }
}
