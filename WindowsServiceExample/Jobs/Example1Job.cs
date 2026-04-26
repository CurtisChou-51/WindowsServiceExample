using AsyncKeyedLock;
using Quartz;
using WindowsServiceExample.Dtos;

namespace WindowsServiceExample.Services
{
    [DisallowConcurrentExecution]
    public class Example1Job : IJob
    {
        private readonly ILogger _logger;
        private readonly AsyncKeyedLocker<string> _locker;

        public Example1Job(ILogger<Example1Job> logger, AsyncKeyedLocker<string> locker)
        {
            _logger = logger;
            _locker = locker;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobScheduleDto? jobScheduleDto = context.JobDetail.JobDataMap.Get("Payload") as JobScheduleDto;
            _logger.LogInformation($"{DateTime.Now:HH:mm:ss} - {jobScheduleDto?.JobName} - start");
            using (await _locker.LockAsync("SharedResource_A"))
            {
                _logger.LogInformation($"{DateTime.Now:HH:mm:ss} - {jobScheduleDto?.JobName} - access SharedResource_A");
                await Task.Delay(12000);
            }
            _logger.LogInformation($"{DateTime.Now:HH:mm:ss} - {jobScheduleDto?.JobName} - end");
            return;
        }
    }
}
