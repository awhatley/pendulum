using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pendulum
{
    /// <summary>
    /// Provides methods to coordinate execution of related actions on individual threads.
    /// </summary>
    public class ThreadController : IDisposable
    {
        private readonly List<Task> _runningTasks = new List<Task>();
        private readonly List<Task> _pendingTasks = new List<Task>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly ManualResetEvent _activity = new ManualResetEvent(false);
        private readonly Dictionary<int, AutoResetEvent> _idleEvents = new Dictionary<int, AutoResetEvent>();
        private readonly ManualResetEvent _abort = new ManualResetEvent(false);

        /// <summary>
        /// Adds an action to the controller. The action is not invoked.
        /// </summary>
        /// <param name="action">An <see cref="Action{ThreadController}"/> delegate to add.</param>
        public void Add(Action<ThreadController> action)
        {
            var task = new Task(() => action(this));
            
            _lock.EnterWriteLock();

            try
            {
                _pendingTasks.Add(task);
                _idleEvents.Add(task.Id, new AutoResetEvent(false));
            }
            
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Starts all pending actions.
        /// </summary>
        public void Start()
        {
            _lock.EnterUpgradeableReadLock();

            try
            {
                if(_pendingTasks.Count > 0)
                {
                    _lock.EnterWriteLock();

                    try
                    {
                        _pendingTasks.ForEach(task => task.Start());
                        _runningTasks.AddRange(_pendingTasks);
                        _pendingTasks.Clear();
                    }

                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }

            finally
            {
                _lock.ExitUpgradeableReadLock();
            }

            _activity.Set();
        }

        /// <summary>
        /// Adds the specified action, starting it immediately.
        /// </summary>
        /// <param name="action">An <see cref="Action{ThreadController}"/> delegate to add and start.</param>
        public void Start(Action<ThreadController> action)
        {
            var task = new Task(() => action(this));

            _lock.EnterWriteLock();

            try
            {
                _idleEvents.Add(task.Id, new AutoResetEvent(false));
                task.Start();
                _runningTasks.Add(task);
            }

            finally
            {
                _lock.ExitWriteLock();
            }

            _activity.Set();
        }

        /// <summary>
        /// Signals all executing actions to pause. This methods blocks until all threads are idle.
        /// </summary>
        public void Pause()
        {
            _activity.Reset();

            AutoResetEvent[] idle;
            _lock.EnterReadLock();

            try
            {
                idle = _idleEvents.Values.ToArray();
            }

            finally
            {
                _lock.ExitReadLock();
            }

            if(idle.Length > 0)
                WaitHandle.WaitAll(idle);
        }

        /// <summary>
        /// Signals all executing actions to resume.
        /// </summary>
        public void Resume()
        {
            _activity.Set();
        }

        /// <summary>
        /// Signals all executing actions to abort, blocking until all threads have completed. If any threads
        /// terminated with an exception, this method will throw an <see cref="AggregateException"/> containing 
        /// all exceptions thrown during execution of child threads.
        /// </summary>
        public void Stop()
        {
            _abort.Set();
            _activity.Set();

            Task[] tasks;
            _lock.EnterReadLock();

            try
            {
                tasks = _runningTasks.ToArray();
            }

            finally
            {
                _lock.ExitReadLock();
            }

            try
            {
                Task.WaitAll(tasks);
            }

            catch(AggregateException ae)
            {
                ae.Flatten().Handle(ex => ex is OperationCanceledException);
            }

            finally
            {
                _lock.EnterWriteLock();

                try
                {
                    _runningTasks.ForEach(t => t.Dispose());
                    _idleEvents.Values.ToList().ForEach(idle => idle.Dispose());

                    _runningTasks.Clear();
                    _pendingTasks.Clear();
                    _idleEvents.Clear();
                }

                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Blocks an executing thread if the controller has signaled for pause, or throws an
        /// <see cref="OperationCanceledException"/> if the controller has signaled abort.
        /// </summary>
        public void Wait()
        {
            Wait(TimeSpan.Zero);
        }

        /// <summary>
        /// Blocks an executing thread for the specified interval if the controller has signaled 
        /// for pause, or throws an <see cref="OperationCanceledException"/> if the controller has 
        /// signaled abort.
        /// </summary>
        /// <param name="timeout">The interval to wait if pause has been signaled, after which
        /// control of the thread is returned.</param>
        public void Wait(TimeSpan timeout)
        {
            AutoResetEvent idleEvent = null;

            _lock.EnterReadLock();

            try
            {
                if(Task.CurrentId.HasValue)
                    _idleEvents.TryGetValue(Task.CurrentId.Value, out idleEvent);
            }

            finally
            {
                _lock.ExitReadLock();
            }

            if(idleEvent != null)
                WaitHandle.SignalAndWait(idleEvent, _activity);

            else
                _activity.WaitOne();

            if(_abort.WaitOne(timeout))
                throw new OperationCanceledException();
        }

        /// <summary>
        /// Calls <see cref="Stop"/> and releases all resources used by the current <see cref="ThreadController"/>.
        /// </summary>
        public void Dispose()
        {
            Stop();

            _activity.Dispose();
            _abort.Dispose();
            _lock.Dispose();
        }
    }
}