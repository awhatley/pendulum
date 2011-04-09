using System.Runtime.Serialization;

namespace Pendulum
{
    /// <summary>
    /// Indicates the status of a scheduled task.
    /// </summary>
    [DataContract]
    public enum TaskStatus
    {
        /// <summary>
        /// The task is ready to execute at its next run time.
        /// </summary>
        [EnumMember]
        Ready,

        /// <summary>
        /// The task is currently executing.
        /// </summary>
        [EnumMember]
        Running,

        /// <summary>
        /// The task previously failed to execute and is waiting to retry.
        /// </summary>
        [EnumMember]
        Retrying,

        /// <summary>
        /// An error occurred while executing the task.
        /// </summary>
        [EnumMember]
        Faulted,
    }
}