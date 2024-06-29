using Microsoft.AspNetCore.Mvc;

namespace JapaneseStudyApi.Quartz;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly TaskInjection _taskInjection;

    public TaskController(TaskInjection taskInjection)
    {
        _taskInjection = taskInjection;
    }

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        var tasks = _taskInjection.GetAllTasks();
        return Ok(tasks);
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleTask(string taskId, string cronExpression)
    {
        await _taskInjection.UpdateTaskSchedule(taskId, cronExpression);
        return Ok(new { message = $"Task {taskId} scheduled." });
    }

    [HttpPost("unschedule")]
    public async Task<IActionResult> UnscheduleTask(string taskId)
    {
        await _taskInjection.UnscheduleTask(taskId);
        return Ok(new { message = $"Task {taskId} unscheduled." });
    }
}
