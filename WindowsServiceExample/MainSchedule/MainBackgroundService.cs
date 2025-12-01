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
        private readonly List<JobScheduleDto> _jobScheduleDtos;
        private IScheduler? _scheduler;

        public MainBackgroundService(ILogger<MainBackgroundService> logger, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobScheduleDtos =
            [
                new JobScheduleDto(jobIdentity: "j1", jobName : "Example1", jobType: typeof(Example1Job), cronExpression : "0/5 * * * * ?"),
                new JobScheduleDto(jobIdentity: "j2", jobName : "Example2", jobType: typeof(Example2Job), cronExpression : "0/20 * * * * ?")
            ];
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

            foreach (JobScheduleDto jobScheduleDto in _jobScheduleDtos)
            {
                IJobDetail jobDetail = CreateJobDetail(jobScheduleDto);
                ITrigger trigger = CreateTrigger(jobScheduleDto);
                await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
            }
            await _scheduler.Start(cancellationToken);
        }

        public async Task<List<JobStatusViewModel>> QueryJobsStatus()
        {
            var currentJobs = await GetCurrentJobs();
            string[] currentJobIds = currentJobs.Select(job => job.JobDetail.Key.Name).ToArray();
            return _jobScheduleDtos.Select(x => 
                new JobStatusViewModel
                {
                    JobIdentity = x.JobIdentity,
                    JobName = x.JobName,
                    IsRunning = currentJobIds.Contains(x.JobIdentity),
                    CronExpression = x.CronExpression,
                }).ToList();
        }

        public void TriggerJob(string jobIdentity)
        {
            _scheduler?.TriggerJob(new JobKey(jobIdentity));
        }

        private async Task<IReadOnlyCollection<IJobExecutionContext>> GetCurrentJobs()
        {
            return _scheduler is null || _scheduler.IsShutdown ? [] : await _scheduler.GetCurrentlyExecutingJobs();
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
