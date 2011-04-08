using System.ServiceModel;

namespace Pendulum.Contracts
{
    /// <summary>
    /// Defines the service contract for schedulable tasks.
    /// </summary>
    [ServiceContract]
    public interface IScheduledTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>A <see cref="TaskExecutionResult"/> instance containing the result of the execution.</returns>
        [OperationContract] 
        TaskExecutionResult Execute();
    }
}