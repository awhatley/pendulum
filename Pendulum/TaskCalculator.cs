using System;

namespace Pendulum
{
    public class TaskCalculator
    {
        public static DateTime CalculateNextRunTime(ScheduledTask task)
        {
            return task.StartDate + TimeSpan.FromTicks(((((DateTime.UtcNow - task.StartDate).Ticks / task.RepeatInterval.Ticks) + 1) * task.RepeatInterval.Ticks)) + CalculateRandomDelay(task);
        }

        public static DateTime CalculateNextRetryTime(ScheduledTask task)
        {
            return DateTime.UtcNow + task.RetryInterval + CalculateRandomDelay(task);
        }

        public static TimeSpan CalculateRandomDelay(ScheduledTask task)
        {
            var random = new Random().NextDouble();
            return TimeSpan.FromTicks((long)(random * task.RandomDelay.Ticks * 2) - task.RandomDelay.Ticks);
        }
    }
}