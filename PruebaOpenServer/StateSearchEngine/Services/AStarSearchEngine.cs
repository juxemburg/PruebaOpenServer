using StateSearchEngine.Interfaces;
using StateSearchEngine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StateSearchEngine.Services
{
    /// <summary>
    /// Clase para búsqueda A*
    /// </summary>
    /// <typeparam name="T">
    /// Tipo del campo identificador del ISearchable a usar
    /// </typeparam>
    public class AStarSearchEngine<T>
        where T : class
    {
        private readonly ISearchable<T> _initialState;
        private readonly ISearchable<T> _goalState;

        private readonly Func<ISearchable<T>, double> _heuristicFn;

        private ISearchable<T> _currentCandidate;
        private readonly bool _returnFirstMatch;

        private Dictionary<T, ISearchable<T>> _discartedQueue;
        private PriorityQueue<ISearchable<T>> _extendQueue;

        private ISearchable<T> _resultCandidate;

        public double ElapsedSearchTime { get; private set; } = 0;
        public double ExtensionsCount { get; private set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">
        /// Estado inicial de la búsqueda
        /// </param>
        /// <param name="goalState">
        /// Estado final de la búsqueda
        /// </param>
        public AStarSearchEngine(ISearchable<T> initialState, ISearchable<T> goalState,
            Func<ISearchable<T>, double> heuristicFn, bool returnFirstMatch = false)
        {
            _returnFirstMatch = returnFirstMatch;

            _initialState = initialState;
            _goalState = goalState;
            _heuristicFn = heuristicFn;

            _extendQueue = new PriorityQueue<ISearchable<T>>();
            _discartedQueue = new Dictionary<T, ISearchable<T>>();
        }

        /// <summary>
        /// Método que realiza la búsqueda por A*
        /// </summary>
        /// <returns>
        /// Una cola con el paso de estados desde el final hasta el inicial
        /// </returns>
        public Queue<ISearchable<T>> ShortestPathSearch()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            _extendQueue.Enqueue(_initialState);

            var resutlt = shortestPathSearch();

            sw.Stop();
            ElapsedSearchTime = sw.ElapsedMilliseconds;
            return resutlt;
        }

        private Queue<ISearchable<T>> shortestPathSearch()
        {
            while (_extendQueue.Count > 0)
            {
                _currentCandidate = _extendQueue.Dequeue();

                if (!_discartedQueue.ContainsKey(_currentCandidate.Id))
                {
                    extendState(_currentCandidate);

                    if (_goalState.Id.Equals(_currentCandidate.Id) && 
                        (_resultCandidate == null || _resultCandidate.Score > _currentCandidate.Score))
                    {
                        _resultCandidate = _currentCandidate;
                        if(_returnFirstMatch)
                        {
                            return shortestPathQueue(_resultCandidate);
                        }
                    }
                }
            };

            // Si no encontró match y no existen elementos en cola para ser extendidos
            // no era posible encontrar un estado adecuado.
            return shortestPathQueue(_resultCandidate);
        }

        private void extendState(ISearchable<T> state)
        {
            ExtensionsCount++;
            if (!_discartedQueue.ContainsKey(state.Id))
            {
                _discartedQueue.Add(state.Id, state);
            }

            foreach (var item in state.Extend())
            {
                item.Score += _heuristicFn(item);
                if(_resultCandidate == null || item.Score < _resultCandidate.Score)
                {
                    _extendQueue.Enqueue(item);
                }
            }
        }


        private Queue<ISearchable<T>> shortestPathQueue(ISearchable<T> winnerState)
        {
            var shortestPath = new Queue<ISearchable<T>>();
            var state = winnerState;
            while (state != null)
            {
                shortestPath.Enqueue(state);
                state = _discartedQueue.ContainsKey(state.ParentId) ? _discartedQueue[state.ParentId] : null;
            }
            return shortestPath;
        }
    }
}
