using System.ServiceProcess;

namespace Pendulum.Service
{
    public class SchedulerService : ServiceBase
    {
        public static void Main()
        {
            Run(new SchedulerService());
        }
    }
}