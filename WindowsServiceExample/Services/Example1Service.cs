using Quartz;
using WindowsServiceExample.Dtos;

namespace WindowsServiceExample.Services
{
    [DisallowConcurrentExecution]
    public class Example1Service : IJob
    {
        private readonly ILogger _logger;

        public Example1Service(ILogger<Example1Service> logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobScheduleDto? jobScheduleDto = context.JobDetail.JobDataMap.Get("Payload") as JobScheduleDto;
            _logger.LogInformation($"{DateTime.Now:HH:mm:ss} - {jobScheduleDto?.JobName} - start");
            await Task.Delay(12000);
            _logger.LogInformation($"{DateTime.Now:HH:mm:ss} - {jobScheduleDto?.JobName} - end");
            return;
        }
    }
}
