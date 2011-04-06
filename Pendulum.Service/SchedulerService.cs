using System.ServiceProcess;

namespace Pendulum.Service
{
    public class SchedulerService : ServiceBase
    {
        internal const string Name = "Pendulum";
        internal const string DisplayName = "Pendulum Scheduler Service";
        internal const string Description = "Sends scheduled activation messages to WCF services requiring execution at regular intervals.";

        private SchedulerService()
        {
            ServiceName = Name;
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        public static void Main()
        {
            Run(new SchedulerService());
        }
    }
}