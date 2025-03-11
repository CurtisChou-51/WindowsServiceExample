using Quartz;
using Quartz.Spi;
using WindowsServiceExample.Dtos;
using WindowsServiceExample.Services.Example1;

namespace WindowsServiceExample
{
    public class ExampleBackgroundService : BackgroundService
    {
        private readonly ILogger<ExampleBackgroundService> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;

        public ExampleBackgroundService(ILogger<ExampleBackgroundService> logger, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
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
            IScheduler scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            scheduler.JobFactory = _jobFactory;

            List<JobScheduleDto> jobScheduleDtos =
            [
                new JobScheduleDto { JobIdentity = "j1", JobName = "Example1", JobType = typeof(Example1Service), CronExpression = "0/5 * * * * ?" }
            ];
            foreach (JobScheduleDto jobScheduleDto in jobScheduleDtos)
            {
                IJobDetail jobDetail = CreateJobDetail(jobScheduleDto);
                ITrigger trigger = CreateTrigger(jobScheduleDto);
                await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            }
            await scheduler.Start(cancellationToken);
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
