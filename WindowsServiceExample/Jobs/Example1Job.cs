using AsyncKeyedLock;
using Quartz;
using WindowsServiceExample.Dtos;

namespace WindowsServiceExample.Jobs
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
            // [DisallowConcurrentExecution] 只防止同一個 Job 類別的多個實例同時執行
            // 如有不同 Job 需要存取相同資源，則可使用 AsyncKeyedLocker 來確保同一時間只有一個 Job 存取
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
