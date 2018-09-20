#region using

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace ThreadGun
{
    public class ThreadGun<T>
    {
        public delegate void CompletedDelegate(IEnumerable<T> inputs);

        public delegate void ExceptionOccurredDelegate(ThreadGun<T> gun, IEnumerable<T> inputs, object input,
            Exception exception);

        private readonly Action _action;
        private readonly Action<T> _actionT;
        private readonly object _activeTasksLock = new object();
        private readonly List<Thread> _activeThreads = new List<Thread>();
        private readonly Func<T, Task> _func;
        private readonly IEnumerable<T> _inputs;
        private readonly int _magazineCount;
        private readonly int _threadCount;
        private List<Task> _activeTasks = new List<Task>();

        private int _completed;
        private ConcurrentStack<Action> _magazine;
        private Action _waitingCompletedAction;
        private int _waitingPeriod;

        public ThreadGun(Func<T, Task> func, IEnumerable<T> inputs, int threadCount,
            CompletedDelegate completedEvent, ExceptionOccurredDelegate exceptionOccurredEvent)
        {
            Completed += completedEvent;
            ExceptionOccurred += exceptionOccurredEvent;
            _threadCount = threadCount;
            _func = func;
            _inputs = inputs;
        }

        public ThreadGun(Func<T, Task> func, IEnumerable<T> inputs, int threadCount) : this(func, inputs, threadCount,
            null, null)
        {
        }

        public ThreadGun(Func<T, Task> func, IEnumerable<T> inputs, int threadCount,
            CompletedDelegate completedEvent) : this(func, inputs, threadCount, completedEvent, null)
        {
        }

        public ThreadGun(Func<T, Task> func, IEnumerable<T> inputs, int threadCount,
            ExceptionOccurredDelegate exceptionOccurredEvent) : this(func, inputs, threadCount, null,
            exceptionOccurredEvent)
        {
        }

        public ThreadGun(Action action, int magazineCount, int threadCount,
            CompletedDelegate completedEvent, ExceptionOccurredDelegate exceptionOccurredEvent)
        {
            Completed += completedEvent;
            ExceptionOccurred += exceptionOccurredEvent;
            _threadCount = threadCount;
            _action = action;
            _magazineCount = magazineCount;
        }

        public ThreadGun(Action action, int magazineCount, int threadCount) : this(action, magazineCount, threadCount,
            null, null)
        {
        }

        public ThreadGun(Action action, int magazineCount, int threadCount,
            CompletedDelegate completedEvent) : this(action, magazineCount, threadCount, completedEvent, null)
        {
        }

        public ThreadGun(Action action, int magazineCount, int threadCount,
            ExceptionOccurredDelegate exceptionOccurredEvent) : this(action, magazineCount, threadCount, null,
            exceptionOccurredEvent)
        {
        }

        public ThreadGun(Action<T> actionT, IEnumerable<T> inputs, int threadCount,
            CompletedDelegate completedEvent, ExceptionOccurredDelegate exceptionOccurredEvent)
        {
            Completed += completedEvent;
            ExceptionOccurred += exceptionOccurredEvent;
            _threadCount = threadCount;
            _actionT = actionT;
            _inputs = inputs;
        }

        public ThreadGun(Action<T> actionT, IEnumerable<T> inputs, int threadCount) : this(actionT, inputs, threadCount,
            null, null)
        {
        }

        public ThreadGun(Action<T> actionT, IEnumerable<T> inputs, int threadCount,
            CompletedDelegate completedEvent) : this(actionT, inputs, threadCount, completedEvent, null)
        {
        }

        public ThreadGun(Action<T> actionT, IEnumerable<T> inputs, int threadCount,
            ExceptionOccurredDelegate exceptionOccurredEvent) : this(actionT, inputs, threadCount, null,
            exceptionOccurredEvent)
        {
        }


        public event CompletedDelegate Completed;
        public event ExceptionOccurredDelegate ExceptionOccurred;

        public void Join()
        {
            CompletedTask();
        }

        public void Wait(int millisecond)
        {
            _waitingPeriod = millisecond;
        }

        public void Wait(int millisecond, Action completeAction)
        {
            _waitingPeriod = millisecond;
            _waitingCompletedAction = completeAction;
        }

        public ThreadGun<T> FillingMagazine(T input)
        {
            if (_action != null) throw new Exception("This action doesn't have input");
            if (_actionT != null)
            {
                void Action()
                {
                    if (ExceptionOccurred == null)
                        _actionT(input);
                    else
                        try
                        {
                            _actionT(input);
                        }
                        catch (Exception ex)
                        {
                            ExceptionOccurred?.Invoke(this, _inputs, input, ex);
                        }

                    if (_waitingPeriod != 0)
                    {
                        Thread.Sleep(_waitingPeriod);
                        _waitingPeriod = 0;
                        _waitingCompletedAction?.Invoke();
                    }

                    try
                    {
                        _magazine.TryPop(out var action);
                        action.Invoke();
                    }
                    catch (NullReferenceException)
                    {
                        _completed = 1;
                    }
                }

                _magazine.Push(Action);
            }
            else if (_func != null)
            {
                async void AsyncAction()
                {
                    var task = Task.Run(async () => await _func(input));
                    AddActiveTask(task);
                    if (ExceptionOccurred == null)
                        await task;
                    else
                        try
                        {
                            await task;
                        }
                        catch (Exception ex)
                        {
                            ExceptionOccurred?.Invoke(this, _inputs, input, ex);
                        }

                    if (_waitingPeriod != 0)
                    {
                        Thread.Sleep(_waitingPeriod);
                        _waitingPeriod = 0;
                        _waitingCompletedAction?.Invoke();
                    }

                    try
                    {
                        _magazine.TryPop(out var action);
                        action.Invoke();
                    }
                    catch (NullReferenceException)
                    {
                        _completed = 1;
                    }
                }

                _magazine.Push(AsyncAction);
            }

            return this;
        }

        public ThreadGun<T> FillingMagazine(int magazineCount)
        {
            if (_action == null) return this;

            void Action()
            {
                if (ExceptionOccurred == null)
                    _action();
                else
                    try
                    {
                        _action();
                    }
                    catch (Exception ex)
                    {
                        ExceptionOccurred?.Invoke(this, null, null, ex);
                    }

                if (_waitingPeriod != 0)
                {
                    Thread.Sleep(_waitingPeriod);
                    _waitingPeriod = 0;
                    _waitingCompletedAction?.Invoke();
                }

                try
                {
                    _magazine.TryPop(out var action);
                    action.Invoke();
                }
                catch (NullReferenceException)
                {
                    _completed = 1;
                }
            }

            for (var i = 0; i < magazineCount; i++)
                _magazine.Push(Action);

            return this;
        }

        public ThreadGun<T> FillingMagazine(Action<T> action, T input)
        {
            void Action()
            {
                if (ExceptionOccurred == null)
                    _actionT(input);
                else
                    try
                    {
                        _actionT(input);
                    }
                    catch (Exception ex)
                    {
                        ExceptionOccurred?.Invoke(this, _inputs, input, ex);
                    }

                if (_waitingPeriod != 0)
                {
                    Thread.Sleep(_waitingPeriod);
                    _waitingPeriod = 0;
                    _waitingCompletedAction?.Invoke();
                }

                try
                {
                    _magazine.TryPop(out var act);
                    act.Invoke();
                }
                catch (NullReferenceException)
                {
                    _completed = 1;
                }
            }

            if (ExceptionOccurred == null)
                _magazine.Push(Action);
            return this;
        }

        public ThreadGun<T> FillingMagazine()
        {
            _magazine = new ConcurrentStack<Action>();
            if (_actionT != null)
            {
                foreach (var input in _inputs)
                {
                    void Action()
                    {
                        if (ExceptionOccurred == null)
                            _actionT(input);
                        else
                            try
                            {
                                _actionT(input);
                            }
                            catch (Exception ex)
                            {
                                ExceptionOccurred?.Invoke(this, _inputs, input, ex);
                            }

                        if (_waitingPeriod != 0)
                        {
                            Thread.Sleep(_waitingPeriod);
                            _waitingPeriod = 0;
                            _waitingCompletedAction?.Invoke();
                        }

                        try
                        {
                            _magazine.TryPop(out var action);
                            action.Invoke();
                        }
                        catch (NullReferenceException)
                        {
                            _completed = 1;
                        }
                    }

                    _magazine.Push(Action);
                }
            }
            else if (_action != null)
            {
                void Action()
                {
                    if (ExceptionOccurred == null)
                        _action();
                    else
                        try
                        {
                            _action();
                        }
                        catch (Exception ex)
                        {
                            ExceptionOccurred?.Invoke(this, null, null, ex);
                        }

                    if (_waitingPeriod != 0)
                    {
                        Thread.Sleep(_waitingPeriod);
                        _waitingPeriod = 0;
                        _waitingCompletedAction?.Invoke();
                    }

                    try
                    {
                        _magazine.TryPop(out var action);
                        action.Invoke();
                    }
                    catch (NullReferenceException)
                    {
                        _completed = 1;
                    }
                }

                for (var i = 0; i < _magazineCount; i++)
                    _magazine.Push(Action);
            }
            else if (_func != null)
            {
                foreach (var input in _inputs)
                {
                    async void AsyncAction()
                    {
                        var task = Task.Run(async () => await _func(input));
                        AddActiveTask(task);
                        if (ExceptionOccurred == null)
                            await task;
                        else
                            try
                            {
                                await task;
                            }
                            catch (Exception ex)
                            {
                                ExceptionOccurred?.Invoke(this, _inputs, input, ex);
                            }

                        if (_waitingPeriod != 0)
                        {
                            Thread.Sleep(_waitingPeriod);
                            _waitingPeriod = 0;
                            _waitingCompletedAction?.Invoke();
                        }

                        try
                        {
                            _magazine.TryPop(out var action);
                            action.Invoke();
                        }
                        catch (NullReferenceException)
                        {
                            _completed = 1;
                        }
                    }

                    _magazine.Push(AsyncAction);
                }
            }

            return this;
        }


        private void CompletedTask()
        {
            while (true)
            {
                lock (_activeTasksLock)
                {
                    _activeTasks = _activeTasks
                        .Where(
                            at => at != null &&
                                  !at.IsCompleted &&
                                  (at.Status != TaskStatus.WaitingToRun ||
                                   at.Status != TaskStatus.WaitingForActivation))
                        .ToList();
                    if (_completed == 1 && _magazine.Count == 0 && _activeTasks.Count == 0)
                    {
                        foreach (var activeThread in _activeThreads) activeThread.Join();
                        Completed?.Invoke(_inputs);
                        return;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void AddActiveTask(Task task)
        {
            lock (_activeTasksLock)
            {
                _activeTasks.Add(task);
            }
        }

        public ThreadGun<T> Start()
        {
            if (_magazine.Count == 0) throw new Exception("Fill the magazine first");
            for (var i = 0; i < _threadCount; i++)
            {
                var thread = new Thread(() =>
                {
                    try
                    {
                        _magazine.TryPop(out var action);
                        action.Invoke();
                    }
                    catch (NullReferenceException)
                    {
                        _completed = 1;
                    }
                });
                _activeThreads.Add(thread);
                thread.Start();
            }

            if (Completed != null)
                new Thread(CompletedTask).Start();
            return this;
        }
    }
}