using System;

namespace Pendulum
{
    public class SchedulePoller
    {
        private readonly TaskRepository _repository;
        private readonly TaskExecutor _executor;

        public SchedulePoller()
        {
            _repository = new TaskRepository();
            _executor = new TaskExecutor(_repository);
        }

        public void Poll(ThreadController controller)
        {
            controller.Wait(); // check for pause/abort

            while(true)
            {
                try
                {
                    var tasks = _repository.GetPendingTasks();
                    foreach(var task in tasks)
                    {
                        var taskToRun = task;
                        controller.Start(c => _executor.Execute(taskToRun, controller));
                    }
                }

                finally
                {
                    // TODO: configurable polling interval
                    controller.Wait(TimeSpan.FromMinutes(1));
                }
            }
        }
    }
}