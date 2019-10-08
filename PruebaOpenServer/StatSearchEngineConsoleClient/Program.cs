using StateSearchEngine.Services;
using StatSearchEngineConsoleClient.DummyClasses;
using System;
using System.Collections.Generic;

namespace StatSearchEngineConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialState = new DummyPkmnCollection(new List<Tuple<int, string, int>> {
                new Tuple<int, string, int>(1, "Bullbasasur", 2),
                new Tuple<int, string, int>(4, "Charmander", 2),
                new Tuple<int, string, int>(7, "Squirtle", 2),
                new Tuple<int, string, int>(10, "Caterpie", 2),
                new Tuple<int, string, int>(16, "Pidgey", 2),
            }, 0, "");

            var goalState = new DummyPkmnCollection(new List<Tuple<int, string, int>> {
                new Tuple<int, string, int>(7, "Squirtle", 2),
                new Tuple<int, string, int>(1, "Bullbasasur", 2),
                new Tuple<int, string, int>(4, "Charmander", 2),
                new Tuple<int, string, int>(10, "Caterpie", 2),
                new Tuple<int, string, int>(16, "Pidgey", 2),
            }, 0, "");

            var searchEngine = new AStarSearchEngine<string>(initialState, goalState);
            var results = searchEngine.ShortestPathSearch();

            Console.WriteLine($"Queue count: {results.Count - 1}");
            Console.WriteLine($"total elapsed time: {searchEngine.ElapsedSearchTime}ms");
            Console.ReadKey();
        }
    }
}
