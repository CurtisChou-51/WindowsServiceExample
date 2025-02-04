namespace WindowsServiceExample.Services.Example1
{
    public class Example1Service : IExample1Service
    {
        private readonly ILogger _logger;

        public Example1Service(ILogger<Example1Service> logger)
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
