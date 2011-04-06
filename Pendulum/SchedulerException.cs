using System;
using System.Collections.Generic;

namespace Pendulum
{
    /// <summary>
    /// Represents one or more unhandled errors that occured during scheduled task execution.
    /// </summary>
    public class SchedulerException : Exception
    {
        /// <summary>
        /// A collection of <see cref="Exception"/> instances that were thrown during execution.
        /// </summary>
        public IEnumerable<Exception> Errors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerException"/> class with the
        /// specified inner <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="inner">An <see cref="AggregateException"/> instance.</param>
        public SchedulerException(AggregateException inner)
            : base("Errors occurred during the execution of scheduled tasks", inner)
        {            
            Errors = inner.InnerExceptions;
        }
    }
}