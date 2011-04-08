using System;

using NUnit.Framework;

namespace Pendulum
{
    [TestFixture]
    public class TaskCalculatorTestFixture
    {
        [Test]
        public void CalculateNextRunTimeTest()
        {
            var task = new ScheduledTask {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0).ToUniversalTime(),
                RepeatInterval = TimeSpan.FromMinutes(60),
            };

            task.NextRunTime = TaskCalculator.CalculateNextRunTime(task);

            var expectedTime = task.StartDate.AddHours(1);

            Assert.That(task.NextRunTime, Is.EqualTo(expectedTime));
        }

        [Test]
        public void CalculateNextRunTimeTest_OldStartDate()
        {
            var task = new ScheduledTask
            {
                StartDate = new DateTime(2000, 5, 16, DateTime.Now.Hour, 0, 0).ToUniversalTime(),
                RepeatInterval = TimeSpan.FromMinutes(60),
            };

            task.NextRunTime = TaskCalculator.CalculateNextRunTime(task);

            var expectedTime = new DateTime(
                DateTime.Now.Year, 
                DateTime.Now.Month, 
                DateTime.Now.Day, 
                DateTime.Now.Hour + 1, 0, 0).ToUniversalTime();

            Assert.That(task.NextRunTime, Is.EqualTo(expectedTime));
        }

        [Test]
        public void CalculateNextRunTimeTest_RandomDelay()
        {
            var task = new ScheduledTask
            {
                StartDate = new DateTime(2000, 5, 16, DateTime.Now.Hour, 0, 0).ToUniversalTime(),
                RepeatInterval = TimeSpan.FromMinutes(60),
                RandomDelay = TimeSpan.FromMinutes(10),
            };

            task.NextRunTime = TaskCalculator.CalculateNextRunTime(task);

            var expectedTime = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour + 1, 0, 0).ToUniversalTime();

            Console.WriteLine(task.NextRunTime.ToLocalTime());

            Assert.That(task.NextRunTime, Is.InRange(expectedTime.AddMinutes(-10), expectedTime.AddMinutes(10)));
        }

        [Test]
        public void CalculateNextRetryTime()
        {
            var task = new ScheduledTask
            {
                StartDate = new DateTime(2000, 5, 16, DateTime.Now.Hour, 0, 0).ToUniversalTime(),
                RepeatInterval = TimeSpan.FromMinutes(60),
                RetryInterval = TimeSpan.FromMinutes(10),
            };

            task.NextRunTime = TaskCalculator.CalculateNextRetryTime(task);

            var expectedTime = DateTime.UtcNow.AddMinutes(10);

            Assert.That(task.NextRunTime, Is.InRange(expectedTime.AddMinutes(-1), expectedTime.AddMinutes(1)));
        }

        [Test]
        public void CalculateNextRunTimeAfterRetry()
        {
            var task = new ScheduledTask
            {
                StartDate = new DateTime(2000, 5, 16, DateTime.Now.Hour, 0, 0).ToUniversalTime(),
                RepeatInterval = TimeSpan.FromMinutes(60),
                RetryInterval = TimeSpan.FromMinutes(10),
            };

            task.NextRunTime = TaskCalculator.CalculateNextRetryTime(task);
            task.NextRunTime = TaskCalculator.CalculateNextRunTime(task);

            var expectedTime = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour + 1, 0, 0).ToUniversalTime();

            Assert.That(task.NextRunTime, Is.EqualTo(expectedTime));
        }

        [Test]
        public void CalculateRandomDelay()
        {
            var task = new ScheduledTask
            {
                StartDate = new DateTime(2000, 5, 16, DateTime.Now.Hour, 30, 22).ToUniversalTime(),
                RepeatInterval = TimeSpan.FromMinutes(15),
                RetryInterval = TimeSpan.FromMinutes(10),
                RandomDelay = TimeSpan.FromMinutes(10),
            };

            for(var i = 0; i < 100; i++)
            {
                var delay = TaskCalculator.CalculateRandomDelay(task);
                System.Threading.Thread.Sleep(1);
                Console.WriteLine(delay);
                Assert.That(delay, Is.InRange(-task.RandomDelay, task.RandomDelay));                
            }
        }
    }
}