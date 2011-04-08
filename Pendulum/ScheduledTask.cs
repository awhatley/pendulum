using System;
using System.Collections.Generic;

namespace Pendulum
{
    public class ScheduledTask
    {
        public bool IsEnabled { get; set; }
        public TaskStatus Status { get; set; }
        public Type BindingType { get; set; }
        public TimeSpan SendTimeout { get; set; }
        public TimeSpan ReceiveTimeout { get; set; }
        public Uri EndpointAddress { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetryCount { get; set; }
        public DateTime NextRunTime { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan RetryInterval { get; set; }
        public TimeSpan RepeatInterval { get; set; }
        public TimeSpan RandomDelay { get; set; }
        public IList<ScheduledTaskExecution> History { get; set; }
    }
}