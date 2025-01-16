namespace WindowsServiceExample.Services
{
    public class Example2Service : IExample2Service
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
