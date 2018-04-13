#region using

using System;
using System.Collections.Generic;
using System.Threading;

#endregion

namespace ThreadGun
{
    public class ThreadGun<T>
    {
        public delegate void CompletedDelegate(IEnumerable<T> inputs);

        public delegate void ExceptionOccurredDelegate(IEnumerable<T> inputs, Exception exception);

        private readonly object _lockObject = new object();
        private readonly List<Action> _magazine;
        private readonly int _threadCount;
        private bool _completed;

        public ThreadGun(Action<T> start, IEnumerable<T> inputs, int threadCount)
        {
            _threadCount = threadCount;
            _magazine = new List<Action>();
            foreach (var input in inputs)
                _magazine.Add(() =>
                {
                    try
                    {
                        start(input);
                    }
                    catch (Exception ex)
                    {
                        ExceptionOccurred?.Invoke(inputs, ex);
                    }

                    try
                    {
                        Action nextAction;
                        lock (_lockObject)
                        {
                            nextAction = _magazine[0];
                            while (!_magazine.Remove(nextAction)) nextAction = _magazine[_magazine.Count / 2];
                        }

                        (nextAction ?? _magazine[0])?.Invoke();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        if (!_completed)
                        {
                            var lockObject = new object();
                            lock (lockObject)
                            {
                                if (!_completed)
                                {
                                    Completed?.Invoke(inputs);
                                    _completed = true;
                                }
                            }
                        }
                    }
                });
        }

        public event CompletedDelegate Completed;
        public event ExceptionOccurredDelegate ExceptionOccurred;

        public void Start()
        {
            for (var i = 0; i < _threadCount; i++)
            {
                Action action;
                lock (_lockObject)
                {
                    action = _magazine[_magazine.Count / 2];
                    _magazine.Remove(action);
                }

                new Thread(() => action()).Start();
            }
        }
    }
}