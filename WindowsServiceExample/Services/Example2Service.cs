using Quartz;
using WindowsServiceExample.Dtos;

namespace WindowsServiceExample.Services
{
    [DisallowConcurrentExecution]
    public class Example2Service : IJob
    {
        private readonly ILogger _logger;

        public Example2Service(ILogger<Example2Service> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            JobScheduleDto? jobScheduleDto = context.JobDetail.JobDataMap.Get("Payload") as JobScheduleDto;
            _logger.LogInformation($"{DateTime.Now:HH:mm:ss} - {jobScheduleDto?.JobName} - start");
            return Task.CompletedTask;
        }
    }
}
