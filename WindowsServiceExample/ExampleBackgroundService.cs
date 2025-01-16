using WindowsServiceExample.Services;

namespace WindowsServiceExample
{
    public class ExampleBackgroundService : BackgroundService
    {
        private readonly ILogger<ExampleBackgroundService> _logger;
        private readonly IEnumerable<IScheduledServiceShell> _scheduledServiceShells;

        public ExampleBackgroundService(ILogger<ExampleBackgroundService> logger, IEnumerable<IScheduledServiceShell> scheduledServiceShells)
        {
            _logger = logger;
            _scheduledServiceShells = scheduledServiceShells;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    foreach (var scheduledServiceShell in _scheduledServiceShells)
                    {
                        if (scheduledServiceShell.IsScheduled(DateTimeOffset.Now))
                            scheduledServiceShell.InvokeService();
                    }
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }
        }
    }
}
