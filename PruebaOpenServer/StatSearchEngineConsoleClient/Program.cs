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
                new Tuple<int, string, int>(1, "A", 2),
                new Tuple<int, string, int>(2, "B", 2),
                new Tuple<int, string, int>(3, "C", 2),
                new Tuple<int, string, int>(4, "D", 2),
                new Tuple<int, string, int>(5, "E", 2),
                new Tuple<int, string, int>(6, "E", 2),
                new Tuple<int, string, int>(7, "E", 2),
                new Tuple<int, string, int>(8, "E", 2),
                new Tuple<int, string, int>(9, "E", 2),
                new Tuple<int, string, int>(10, "E", 2),
                new Tuple<int, string, int>(11, "A", 2),
                new Tuple<int, string, int>(12, "B", 2),
                new Tuple<int, string, int>(13, "C", 2),
                new Tuple<int, string, int>(14, "D", 2),
                new Tuple<int, string, int>(15, "E", 2),
                new Tuple<int, string, int>(16, "E", 2),
                new Tuple<int, string, int>(17, "E", 2),
                new Tuple<int, string, int>(18, "E", 2),
                new Tuple<int, string, int>(19, "E", 2),
                new Tuple<int, string, int>(20, "E", 2),
                new Tuple<int, string, int>(21, "A", 2),
                new Tuple<int, string, int>(22, "B", 2),
                new Tuple<int, string, int>(23, "C", 2),
                new Tuple<int, string, int>(24, "D", 2),
                new Tuple<int, string, int>(25, "E", 2),
                new Tuple<int, string, int>(26, "E", 2),
                new Tuple<int, string, int>(27, "E", 2),
                new Tuple<int, string, int>(28, "E", 2),
                new Tuple<int, string, int>(29, "E", 2),
                new Tuple<int, string, int>(30, "E", 2)
            }, 0, "");

            var goalState = new DummyPkmnCollection(new List<Tuple<int, string, int>> {
                new Tuple<int, string, int>(1, "A", 2),
                new Tuple<int, string, int>(2, "B", 2),
                new Tuple<int, string, int>(3, "C", 2),
                new Tuple<int, string, int>(5, "E", 2),
                new Tuple<int, string, int>(4, "D", 2),
                new Tuple<int, string, int>(7, "E", 2),
                new Tuple<int, string, int>(6, "E", 2),
                new Tuple<int, string, int>(9, "E", 2),
                new Tuple<int, string, int>(8, "E", 2),
                new Tuple<int, string, int>(10, "E", 2),
                new Tuple<int, string, int>(11, "A", 2),
                new Tuple<int, string, int>(12, "B", 2),
                new Tuple<int, string, int>(13, "C", 2),
                new Tuple<int, string, int>(14, "D", 2),
                new Tuple<int, string, int>(15, "E", 2),
                new Tuple<int, string, int>(17, "E", 2),
                new Tuple<int, string, int>(16, "E", 2),
                new Tuple<int, string, int>(18, "E", 2),
                new Tuple<int, string, int>(19, "E", 2),
                new Tuple<int, string, int>(20, "E", 2),
                new Tuple<int, string, int>(21, "A", 2),
                new Tuple<int, string, int>(22, "B", 2),
                new Tuple<int, string, int>(23, "C", 2),
                new Tuple<int, string, int>(25, "E", 2),
                new Tuple<int, string, int>(24, "D", 2),
                new Tuple<int, string, int>(26, "E", 2),
                new Tuple<int, string, int>(27, "E", 2),
                new Tuple<int, string, int>(28, "E", 2),
                new Tuple<int, string, int>(29, "E", 2),
                new Tuple<int, string, int>(30, "E", 2)
            }, 0, "");

            //var initialState = new DummyPkmnCollection(new List<Tuple<int, string, int>> {
            //    new Tuple<int, string, int>(1, "Bullbasasur", 2),
            //    new Tuple<int, string, int>(4, "Charmander", 2),
            //    new Tuple<int, string, int>(7, "Squirtle", 2),
            //    new Tuple<int, string, int>(10, "Caterpie", 2),
            //    new Tuple<int, string, int>(16, "Pidgey", 2),
            //}, 0, "");

            //var goalState = new DummyPkmnCollection(new List<Tuple<int, string, int>> {
            //    new Tuple<int, string, int>(7, "Squirtle", 2),
            //    new Tuple<int, string, int>(1, "Bullbasasur", 2),
            //    new Tuple<int, string, int>(4, "Charmander", 2),
            //    new Tuple<int, string, int>(10, "Caterpie", 2),
            //    new Tuple<int, string, int>(16, "Pidgey", 2),
            //}, 0, "");

            var goalStatePkmDict = goalState.PkmnPositions;

            var searchEngine = new AStarSearchEngine<string>(initialState, goalState, (pkCollection) =>
            {
                var pkDict = (pkCollection as DummyPkmnCollection).PkmnPositions;
                var score = 0;
                foreach (var key in pkDict.Keys)
                {
                    score += Math.Abs(pkDict[key] - goalStatePkmDict[key]);
                }
                return score / 2;
            }, false);
            var results = searchEngine.ShortestPathSearch();

            if (results != null && results.Count > 0)
            {
                Console.WriteLine($"Movement count: {results.Count - 1}");
                Console.WriteLine();
                while (results.Count > 0)
                {
                    Console.WriteLine($"Step: {results.Count - 1}: {results.Dequeue().Id}");
                }
                Console.WriteLine();
                Console.WriteLine($"total extensions: {searchEngine.ExtensionsCount}");
                Console.WriteLine($"total elapsed time: {searchEngine.ElapsedSearchTime}ms");
            }
            else
            {
                Console.WriteLine($"total extensions: {searchEngine.ExtensionsCount}");
                Console.WriteLine($"The search yielded no results");
            }


            Console.ReadKey();
        }
    }
}
