using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JapaneseStudyApi.Quartz.Interface;
using JapaneseStudyApi.Quartz.Tasks;
using Quartz;

namespace JapaneseStudyApi.Quartz
{
    public class TaskInjection
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<IBaseTask> _tasks;
        private readonly IScheduler _scheduler;

        public TaskInjection(IServiceProvider serviceProvider, ISchedulerFactory schedulerFactory)
        {
            _serviceProvider = serviceProvider;
            _tasks = new List<IBaseTask>();
            _scheduler = schedulerFactory.GetScheduler().Result;

            RegisterTask<ExampleTask>("0/5 * * * * ?");
            RegisterTask<AnotherTask>(); // Not scheduled
        }

        public void RegisterTask<T>(string cronExpression = null) where T : IBaseTask
        {
            var task = _serviceProvider.GetRequiredService<T>();
            task.CronExpression = cronExpression;
            task.IsScheduled = !string.IsNullOrEmpty(cronExpression);
            _tasks.Add(task);

            if (task.IsScheduled)
            {
                ScheduleTask(task).Wait();
            }
        }

        public async Task ScheduleTask(IBaseTask task)
        {
            var jobDetail = JobBuilder.Create<GenericJob>()
                .WithIdentity(task.Id)
                .UsingJobData("TaskId", task.Id)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{task.Id}.trigger")
                .WithCronSchedule(task.CronExpression)
                .Build();

            await _scheduler.ScheduleJob(jobDetail, trigger);
        }

        public async Task UnscheduleTask(string taskId)
        {
            var jobKey = new JobKey(taskId);
            await _scheduler.DeleteJob(jobKey);

            var task = _tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.IsScheduled = false;
                task.CronExpression = null;
            }
        }

        public List<IBaseTask> GetAllTasks()
        {
            return _tasks;
        }

        public IBaseTask GetTaskById(string taskId)
        {
            return _tasks.Find(t => t.Id == taskId);
        }

        public async Task UpdateTaskSchedule(string taskId, string cronExpression)
        {
            var task = _tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                await UnscheduleTask(taskId);
                task.CronExpression = cronExpression;
                task.IsScheduled = true;
                await ScheduleTask(task);
            }
        }

        public async Task RunStartupTasksAsync()
        {
            foreach (var task in _tasks)
            {
                if (!task.IsScheduled)
                {
                    await task.ExecuteAsync();
                }
            }
        }
    }
}