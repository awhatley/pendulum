namespace Pendulum
{
    /// <summary>
    /// Indicates the status of a scheduled task.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// The task is ready to execute at its next run time.
        /// </summary>
        Ready,

        /// <summary>
        /// The task is currently executing.
        /// </summary>
        Running,

        /// <summary>
        /// The task previously failed to execute and is waiting to retry.
        /// </summary>
        Retrying,

        /// <summary>
        /// An error occurred while executing the task.
        /// </summary>
        Faulted,
    }
}