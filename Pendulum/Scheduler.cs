using System;

namespace Pendulum
{
    /// <summary>
    /// Controls execution of scheduled tasks.
    /// </summary>
    public class Scheduler : IDisposable
    {
        private readonly ThreadController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scheduler"/> class.
        /// </summary>
        public Scheduler()
        {
            _controller = new ThreadController();
        }

        /// <summary>
        /// Starts scheduling polling.
        /// </summary>
        public void Start()
        {
            _controller.Start();
        }

        /// <summary>
        /// Pauses schedule polling.
        /// </summary>
        public void Pause()
        {
            _controller.Pause();
        }

        /// <summary>
        /// Resumes schedule polling.
        /// </summary>
        public void Resume()
        {
            _controller.Resume();
        }

        /// <summary>
        /// Stops the scheduler and releases any associated managed resources.
        /// </summary>
        public void Stop()
        {
            try
            {
                _controller.Stop();
            }

            catch(AggregateException ex)
            {
                throw new SchedulerException(ex);
            }

            finally
            {
                _controller.Dispose();
            }
        }

        void IDisposable.Dispose()
        {
            Stop();
        }
    }
}