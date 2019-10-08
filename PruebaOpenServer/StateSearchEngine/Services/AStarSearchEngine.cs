using StateSearchEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

        private ISearchable<T> _currentCandidate;
        private int _currentIdx;

        private Dictionary<T, ISearchable<T>> _discartedQueue;
        private List<ISearchable<T>> _extendedQueue;

        public double ElapsedSearchTime { get; private set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">
        /// Estado inicial de la búsqueda
        /// </param>
        /// <param name="goalState">
        /// Estado final de la búsqueda
        /// </param>
        public AStarSearchEngine(ISearchable<T> initialState, ISearchable<T> goalState)
        {
            _initialState = initialState;
            _goalState = goalState;

            _extendedQueue = new List<ISearchable<T>>();
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

            _currentCandidate = _initialState;
            _currentIdx = 0;
            _extendedQueue.Add(_currentCandidate);

            var resutlt = shortestPathSearch();

            sw.Stop();
            ElapsedSearchTime = sw.ElapsedMilliseconds;
            return resutlt;
        }

        private Queue<ISearchable<T>> shortestPathSearch()
        {
            while (_extendedQueue.Count > 0)
            {
                _currentCandidate = getNextCandidate();
                extendState(_currentCandidate, _currentIdx);

                // Encontró el candidato más corto
                if (_goalState.Id.Equals(_currentCandidate.Id))
                {
                    return shortestPathQueue(_currentCandidate);
                }
            };

            // Si no encontró match y no existen elementos en cola para ser extendidos
            // no era posible encontrar un estado adecuado.
            return null;
        }

        private void extendState(ISearchable<T> state, int stateIndex)
        {
            _extendedQueue.RemoveAt(stateIndex);
            if (!_discartedQueue.ContainsKey(state.Id))
            {
                _discartedQueue.Add(state.Id, state);
            }

            foreach (var item in state.Extend())
            {
                if (!_discartedQueue.ContainsKey(item.Id))
                {
                    _extendedQueue.Add(item);
                }
            }
        }

        private ISearchable<T> getNextCandidate()
        {
            if (_extendedQueue.Count <= 0)
            {
                return null;
            }

            
            double minValue = double.MaxValue;

            var idx = 0;
            foreach (var item in _extendedQueue)
            {
                if (item.Score < minValue)
                {
                    minValue = item.Score;
                    _currentCandidate = item;
                    _currentIdx = idx;
                }
                idx++;
            }

            return _currentCandidate;
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
