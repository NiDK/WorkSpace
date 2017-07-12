using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PwC.C4.ConnectionPool.Util
{
    internal class CumulativeCounter
    {
        private int _current;

        private int[] _history;
        private int _cursor;

        private int _total;

        #region ctor

        internal CumulativeCounter(int count)
        {
            _history = new int[count];
        }

        #endregion

        internal void Increment()
        {
            Interlocked.Increment(ref _current);
        }

        #region Lock

        private const int LockFree = 0;
        private const int LockOwned = 1;

        private int _lock = LockFree;

        private bool EnterLock()
        {
            Thread.BeginCriticalRegion();

            // If resource available, set it to in-use and return.
            if (Interlocked.Exchange(ref _lock, LockOwned) == LockFree)
            {
                return true;
            }
            else
            {
                Thread.EndCriticalRegion();
                return false;
            }
        }

        private void ExitLock()
        {
            // Mark the resource as available.
            Interlocked.Exchange(ref _lock, LockFree);
            Thread.EndCriticalRegion();
        }

        #endregion

        /// <summary>
        /// Grabs the accumulated amount in the counter and clears it.
        /// This needs to be called by a 1 second timer.
        /// </summary>
        /// <returns>The total accumulated over the last minute.</returns>
        internal int Tick()
        {
            if (EnterLock())
            {
                try
                {
                    int current = Interlocked.Exchange(ref _current, 0);
                    int previous = Interlocked.Exchange(ref _history[_cursor], current);

                    _cursor = (_cursor + 1) % _history.Length;

                    _total -= previous;
                    _total += current;

                    return _total;
                }
                finally
                {
                    ExitLock();
                }
            }

            return -1;
        }
    }
}
