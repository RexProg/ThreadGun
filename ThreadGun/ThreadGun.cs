#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#endregion

namespace ThreadGun
{
    public class ThreadGun<T>
    {
        public delegate void CompletedDelegate(IEnumerable<T> inputs);

        public delegate void ExceptionOccurredDelegate(IEnumerable<T> inputs, Exception exception);

        private readonly List<Thread> _activeThreads = new List<Thread>();

        private readonly object _lockObject = new object();
        private readonly List<Action> _magazine;
        private readonly int _threadCount;
        private bool _completed;

        public ThreadGun(Action<T> start, IEnumerable<T> inputs, int threadCount)
        {
            _threadCount = threadCount;
            _magazine = new List<Action>();
            var enumerable = inputs as T[] ?? inputs.ToArray();
            foreach (var input in enumerable)
                _magazine.Add(() =>
                {
                    try
                    {
                        start(input);
                    }
                    catch (Exception ex)
                    {
                        ExceptionOccurred?.Invoke(enumerable, ex);
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
                                    Completed?.Invoke(enumerable);
                                    _completed = true;
                                }
                            }
                        }
                    }
                });
        }

        public int ActiveThread
        {
            get { return _activeThreads.Count(t => t.IsAlive); }
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

                var thread = new Thread(() => action());
                _activeThreads.Add(thread);
                thread.Start();
            }
        }
    }
}