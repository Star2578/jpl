using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JapaneseStudyApi.Quartz.Interface
{
    public interface IBaseTask
    {
        Task ExecuteAsync();
        string Id { get; }
        string CronExpression { get; set; }
        bool IsScheduled { get; set; }
    }
}