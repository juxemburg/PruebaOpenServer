using System;
using System.Collections.Generic;
using System.Text;

namespace PokeServices.PokedexServices
{
    public static class PokedexListExtensions
    {
        private static readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int increment = _random.Next(2);
                int k = i + increment;

                if (k < list.Count)
                {
                    T value = list[k];
                    list[k] = list[i];
                    list[i] = value;
                }
                i += increment;
            }
            
        }
    }
}
