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

        public delegate void ExceptionOccurredDelegate(IEnumerable<T> inputs, T input, Exception exception);

        private readonly Action<T> _action;
        private readonly List<Thread> _activeThreads = new List<Thread>();
        private readonly IEnumerable<T> _inputs;
        private readonly int _threadCount;
        private bool _completed;
        private Stack<Action> _magazine;

        public ThreadGun(Action<T> action, IEnumerable<T> inputs, int threadCount,
            CompletedDelegate completedEvent = null, ExceptionOccurredDelegate exceptionOccurredEvent = null)
        {
            Completed += completedEvent;
            ExceptionOccurred += exceptionOccurredEvent;
            _threadCount = threadCount;
            _action = action;
            _inputs = inputs;
        }

        public int ActiveThread
        {
            get { return _activeThreads.Count(t => t.IsAlive); }
        }

        public event CompletedDelegate Completed;
        public event ExceptionOccurredDelegate ExceptionOccurred;

        public void Start()
        {
            _magazine = new Stack<Action>();
            var enumerable = _inputs as T[] ?? _inputs.ToArray();
            foreach (var input in enumerable)
            {
                void ExceptionManagementAction()
                {
                    try
                    {
                        _action(input);
                    }
                    catch (Exception ex)
                    {
                        ExceptionOccurred?.Invoke(enumerable, input, ex);
                    }

                    try
                    {
                        var nextAction = _magazine.Pop();
                        nextAction.Invoke();
                    }
                    catch (InvalidOperationException)
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
                }

                void ExceptionMismanagementAction()
                {
                    _action(input);

                    try
                    {
                        var nextAction = _magazine.Pop();
                        nextAction.Invoke();
                    }
                    catch (InvalidOperationException)
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
                }

                if (ExceptionOccurred == null)
                    _magazine.Push(ExceptionMismanagementAction);
                else
                    _magazine.Push(ExceptionManagementAction);
            }

            for (var i = 0; i < _threadCount; i++)
            {
                var action = _magazine.Pop();
                var thread = new Thread(() => action());
                _activeThreads.Add(thread);
                thread.Start();
            }
        }
    }
}