using Quartz;
using Quartz.Spi;
using WindowsServiceExample.Dtos;
using WindowsServiceExample.Services;

namespace WindowsServiceExample.MainSchedule
{
    public class MainBackgroundService : BackgroundService
    {
        private readonly ILogger<MainBackgroundService> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private IScheduler? _scheduler;

        public MainBackgroundService(ILogger<MainBackgroundService> logger, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await StartScheduler(stoppingToken);
                while (!stoppingToken.IsCancellationRequested)
                    await Task.Delay(1000, stoppingToken);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Error occurred during execution");
            }
        }

        private async Task StartScheduler(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;

            List<JobScheduleDto> jobScheduleDtos =
            [
                new JobScheduleDto(jobIdentity: "j1", jobName : "Example1", jobType: typeof(Example1Service), cronExpression : "0/5 * * * * ?"),
                new JobScheduleDto(jobIdentity: "j2", jobName : "Example2", jobType: typeof(Example2Service), cronExpression : "0/20 * * * * ?")
            ];
            foreach (JobScheduleDto jobScheduleDto in jobScheduleDtos)
            {
                IJobDetail jobDetail = CreateJobDetail(jobScheduleDto);
                ITrigger trigger = CreateTrigger(jobScheduleDto);
                await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            }
            await _scheduler.Start(cancellationToken);
        }

        private static IJobDetail CreateJobDetail(JobScheduleDto jobScheduleDto)
        {
            Type jobType = jobScheduleDto.JobType;
            IJobDetail jobDetail = JobBuilder
                .Create(jobType)
                .WithIdentity(jobScheduleDto.JobIdentity)
                .WithDescription(jobScheduleDto.JobName)
                .Build();

            jobDetail.JobDataMap.Put("Payload", jobScheduleDto);
            return jobDetail;
        }

        private static ITrigger CreateTrigger(JobScheduleDto jobScheduleDto)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{jobScheduleDto.JobIdentity}.trigger")
                .WithCronSchedule(jobScheduleDto.CronExpression)
                .WithDescription(jobScheduleDto.CronExpression)
                .Build();
        }
    }
}
