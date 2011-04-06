using System;

namespace Pendulum
{
    public class SchedulePoller
    {
        private TaskRepository _repository;

        public SchedulePoller()
        {
            _repository = new TaskRepository();
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
                        // TODO: execute task
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