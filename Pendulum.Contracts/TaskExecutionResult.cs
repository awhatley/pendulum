using System.Runtime.Serialization;

namespace Pendulum.Contracts
{
    /// <summary>
    /// Defines the data contract for reporting results of a task execution.
    /// </summary>
    [DataContract]
    public class TaskExecutionResult : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets a value indicating the result of a task.
        /// </summary>
        [DataMember]
        public TaskResult Result { get; set; }

        /// <summary>
        /// Gets or sets an optional message describing the result of a task.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the structure that contains extra data.
        /// </summary>
        ExtensionDataObject IExtensibleDataObject.ExtensionData { get; set; }
    }
}