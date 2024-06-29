using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace JapaneseStudyApi.Quartz
{
    public class GenericJob : IJob
    {
        private readonly TaskInjection _taskInjection;

        public GenericJob(TaskInjection taskInjection)
        {
            _taskInjection = taskInjection;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var taskId = context.JobDetail.JobDataMap.GetString("TaskId");
            var task = _taskInjection.GetTaskById(taskId);
            if (task != null)
            {
                await task.ExecuteAsync();
            }
        }
    }
}