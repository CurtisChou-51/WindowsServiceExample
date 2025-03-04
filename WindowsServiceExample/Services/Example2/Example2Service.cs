namespace WindowsServiceExample.Services.Example2
{
    public class Example2Service
    {
        private readonly ILogger _logger;

        public Example2Service(ILogger<Example2Service> logger)
        {
            _logger = logger;
        }

        public async Task Execute()
        {
            _logger.LogInformation("Execution started.");


            _logger.LogInformation("Execution finished.");
        }
    }
}
