using System;
using System.Runtime.Serialization;

using Pendulum.Contracts;

namespace Pendulum
{
    [DataContract]
    public class ScheduledTaskExecution
    {
        [DataMember] public DateTime RunTime { get; set; }
        [DataMember] public TaskResult Result { get; set; }
        [DataMember] public string Message { get; set; }
    }
}