using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Pendulum.Service
{
    [RunInstaller(true)]
    public class SchedulerServiceInstaller : Installer
    {
        readonly ServiceProcessInstaller _process;
        readonly ServiceInstaller _service;

        public SchedulerServiceInstaller()
        {
            _process = new ServiceProcessInstaller {
                Account = ServiceAccount.LocalSystem
            };

            _service = new ServiceInstaller {
                ServiceName = SchedulerService.Name, 
                DisplayName = SchedulerService.DisplayName, 
                Description = SchedulerService.Description,
                StartType = ServiceStartMode.Automatic,
            };

            Installers.Add(_process);
            Installers.Add(_service);
        }
    }
}