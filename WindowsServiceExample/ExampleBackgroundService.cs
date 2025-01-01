using WindowsServiceExample.Services;

namespace WindowsServiceExample
{
    public class ExampleBackgroundService : BackgroundService
    {
        private readonly ILogger<ExampleBackgroundService> _logger;
        private readonly IEnumerable<IBgShellService> _bgShellServices;

        public ExampleBackgroundService(ILogger<ExampleBackgroundService> logger, IEnumerable<IBgShellService> bgShellServices)
        {
            _logger = logger;
            _bgShellServices = bgShellServices;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    foreach (var bgShellService in _bgShellServices)
                        bgShellService.Execute();
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
