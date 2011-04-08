using System;

using Pendulum.Contracts;

namespace Pendulum
{
    public class ScheduledTaskExecution
    {
        public DateTime RunTime { get; set; }
        public TaskResult Result { get; set; }
        public string Message { get; set; }
    }
}