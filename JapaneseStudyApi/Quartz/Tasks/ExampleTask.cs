using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JapaneseStudyApi.Quartz.Interface;

namespace JapaneseStudyApi.Quartz.Tasks
{
    public class ExampleTask : IBaseTask
    {
        public string Id { get; } = "ExampleTask";
        public string CronExpression { get; set; }
        public bool IsScheduled { get; set; }

        public Task ExecuteAsync()
        {
            Console.WriteLine($"ExampleTask executed at: {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}