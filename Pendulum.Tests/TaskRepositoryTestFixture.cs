using System;
using System.Linq;

using NUnit.Framework;

namespace Pendulum
{
    [TestFixture]
    public class TaskRepositoryTestFixture
    {
        [Test]
        public void Save()
        {
            var repository = new TaskRepository();
            repository.Save(new ScheduledTask { 
                Name = "Test Task", 
                EndpointAddress = new Uri("http://localhost/") 
            });

            repository.Save(new ScheduledTask { 
                Name = "Test Task", 
                EndpointAddress = new Uri("http://localhost/"),
                Status = TaskStatus.Faulted
            });

            repository.Save(new ScheduledTask { 
                Name = "Test Task 1234", 
                EndpointAddress = new Uri("http://localhost/1234") 
            });
        }

        [Test]
        public void GetPendingTasks()
        {
            var repository = new TaskRepository();
            var tasks = repository.GetPendingTasks().ToList();
            foreach(var task in tasks)
            {
                Console.WriteLine(task.Name);
            }
        }
    }
}