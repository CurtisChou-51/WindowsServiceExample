using Quartz;

namespace WindowsServiceExample.Services.Example1
{
    [DisallowConcurrentExecution]
    public class Example1Service : IJob
    {
        private readonly ILogger _logger;

        public Example1Service(ILogger<Example1Service> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            //var schedule = context.JobDetail.JobDataMap.Get("Payload") as JobSchedule;
            _logger.LogInformation($"{DateTime.Now:HH:mm:ss} - Example1Service - start");
            return Task.CompletedTask;
        }
    }
}
