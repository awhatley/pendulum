using System.Runtime.Serialization;

namespace Pendulum.Contracts
{
    /// <summary>
    /// Indicates the result of a task execution.
    /// </summary>
    [DataContract]
    public enum TaskResult
    {
        /// <summary>
        /// The task execution completed successfully.
        /// </summary>
        [EnumMember]
        Success,

        /// <summary>
        /// A recoverable error occurred during execution and the task will be retried.
        /// </summary>
        [EnumMember]
        RecoverableFault,

        /// <summary>
        /// A fatal error occurred during execution and the task will be disabled.
        /// </summary>
        [EnumMember]
        FatalError,
    }
}