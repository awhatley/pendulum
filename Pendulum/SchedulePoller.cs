using System;

namespace Pendulum
{
    public class SchedulePoller
    {
        public void Poll(ThreadController controller)
        {
            controller.Wait(); // check for pause/abort

            while(true)
            {
                try
                {
                    // TODO: get task details and execute
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