using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Pendulum.Contracts
{
    /// <summary>
    /// Provides a WCF client proxy for communicating with <see cref="IScheduledTask"/> service instances.
    /// </summary>
    public class ScheduledTaskClient : ClientBase<IScheduledTask>, IScheduledTask
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskClient"/> class using the
        /// default target endpoint from the application configuration file.
        /// </summary>
        public ScheduledTaskClient() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskClient"/> class using the
        /// configuration information specified in the application configuration file by name.
        /// </summary>
        /// <param name="endpointConfigurationName">The name of the endpoint in the application configuration file.</param>
        public ScheduledTaskClient(string endpointConfigurationName) : 
            base(endpointConfigurationName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskClient"/> class using the
        /// configuration information specified in the application configuration file by name
        /// and the specified target address.
        /// </summary>
        /// <param name="endpointConfigurationName">The name of the endpoint in the application configuration file.</param>
        /// <param name="remoteAddress">The address of the service.</param>
        public ScheduledTaskClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskClient"/> class using the
        /// configuration information specified in the application configuration file by name
        /// and the specified target address.
        /// </summary>
        /// <param name="endpointConfigurationName">The name of the endpoint in the application configuration file.</param>
        /// <param name="remoteAddress">The address of the service.</param>
        public ScheduledTaskClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskClient"/> class using the
        /// specified binding and target address.
        /// </summary>
        /// <param name="binding">The binding with which to make calls to the service.</param>
        /// <param name="remoteAddress">The address of the service.</param>
        public ScheduledTaskClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskClient"/> class using the
        /// specified <see cref="ServiceEndpoint"/>.
        /// </summary>
        /// <param name="endpoint">The endpoint description for the service.</param>
        public ScheduledTaskClient(ServiceEndpoint endpoint) 
            : base(endpoint) { }

        #endregion

        #region IScheduledTask Members

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>A <see cref="TaskExecutionResult"/> instance containing the result of the execution.</returns>
        public TaskExecutionResult Execute()
        {
            return Channel.Execute();
        }

        #endregion
    }
}