using Quartz.Spi;
using Quartz;

namespace WindowsServiceExample.MainSchedule
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Type jobType = bundle.JobDetail.JobType;
            return _serviceProvider.GetRequiredService(jobType) as IJob ?? throw new InvalidOperationException($"Could not create job {jobType}");
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
