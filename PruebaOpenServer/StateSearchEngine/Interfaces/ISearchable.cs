using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StateSearchEngine.Interfaces
{
    public interface ISearchable<T>: IComparable<ISearchable<T>>
    {
        T Id { get; set; }

        T ParentId { get; set; }

        double Score { get; set; }

        IEnumerable<ISearchable<T>> Extend();

    }
}
