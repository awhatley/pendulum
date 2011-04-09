using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Pendulum
{
    [DataContract]
    public class ScheduledTask
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public bool IsEnabled { get; set; }
        [DataMember] public TaskStatus Status { get; set; }
        [DataMember] public string BindingType { get; set; }
        [DataMember] public TimeSpan SendTimeout { get; set; }
        [DataMember] public TimeSpan ReceiveTimeout { get; set; }
        [DataMember] public Uri EndpointAddress { get; set; }
        [DataMember] public int RetryCount { get; set; }
        [DataMember] public int MaxRetryCount { get; set; }
        [DataMember] public DateTime StartDate { get; set; }
        [DataMember] public DateTime ExpirationDate { get; set; }
        [DataMember] public DateTime NextRunTime { get; set; }
        [DataMember] public TimeSpan RetryInterval { get; set; }
        [DataMember] public TimeSpan RepeatInterval { get; set; }
        [DataMember] public TimeSpan RandomDelay { get; set; }
        [DataMember] public IList<ScheduledTaskExecution> History { get; private set; }

        public ScheduledTask()
        {
            IsEnabled = true;
            Status = TaskStatus.Ready;
            BindingType = typeof(BasicHttpBinding).AssemblyQualifiedName;
            SendTimeout = TimeSpan.FromSeconds(30);
            ReceiveTimeout = TimeSpan.FromSeconds(30);
            RetryCount = 0;
            MaxRetryCount = 0;
            StartDate = DateTime.Now;
            ExpirationDate = DateTime.MaxValue;
            NextRunTime = DateTime.Now;
            RetryInterval = TimeSpan.Zero;
            RepeatInterval = TimeSpan.Zero;
            RandomDelay = TimeSpan.Zero;
            History = new List<ScheduledTaskExecution>();
        }
    }
}