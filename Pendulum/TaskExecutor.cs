using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

using Pendulum.Contracts;

namespace Pendulum
{
    public class TaskExecutor
    {
        private readonly TaskRepository _repository;

        public TaskExecutor(TaskRepository repository)
        {
            _repository = repository;
        }

        public void Execute(ScheduledTask task, ThreadController controller)
        {
            controller.Wait();

            task.Status = TaskStatus.Running;
            _repository.Save(task);

            var client = CreateClient(task);

            controller.Wait();

            try
            {
                client.Open();
                var result = client.Execute();
                client.Close();

                RecordResult(task, result.Result, result.Message);
            }
                
            catch(TimeoutException te)
            {
                RecordResult(task, TaskResult.RecoverableFault, te.Message, te);
                client.Abort();
            }

            catch(CommunicationException ce)
            {
                RecordResult(task, TaskResult.RecoverableFault, ce.Message, ce);
                client.Abort();
            }

            catch(Exception ex)
            {
                RecordResult(task, TaskResult.FatalError, ex.Message, ex);
                client.Abort();
                throw;
            }
        }

        private ScheduledTaskClient CreateClient(ScheduledTask task)
        {
            try
            {
                var binding = (Binding)Activator.CreateInstance(task.BindingType);
                binding.SendTimeout = task.SendTimeout;
                binding.ReceiveTimeout = task.ReceiveTimeout;
                
                var endpoint = new EndpointAddress(task.EndpointAddress);
                return new ScheduledTaskClient(binding, endpoint);
            }

            catch(Exception ex)
            {
                RecordResult(task, TaskResult.FatalError, "Error creating client proxy", ex);
                throw;
            }
        }

        private void RecordResult(ScheduledTask task, TaskResult result, string message, Exception exception = null)
        {
            if(exception != null)
                message += Environment.NewLine + Environment.NewLine + exception.Message +
                    Environment.NewLine + Environment.NewLine + exception.StackTrace;

            switch(result)
            {
                case TaskResult.Success:
                    task.RetryCount = 0;
                    task.Status = TaskStatus.Ready;
                    task.NextRunTime = TaskCalculator.CalculateNextRunTime(task);
                    break;

                case TaskResult.RecoverableFault:
                    if(task.RetryCount >= task.MaxRetryCount)
                        goto case TaskResult.FatalError;

                    task.RetryCount += 1;
                    task.Status = TaskStatus.Retrying;
                    task.NextRunTime = TaskCalculator.CalculateNextRetryTime(task);
                    break;

                case TaskResult.FatalError:
                    task.IsEnabled = false;
                    task.RetryCount += 1;
                    task.Status = TaskStatus.Faulted;
                    break;
            }

            task.History.Add(new ScheduledTaskExecution {
                Message = message,
                Result = result,
                RunTime = DateTime.UtcNow,
            });

            _repository.Save(task);
        }
    }
}