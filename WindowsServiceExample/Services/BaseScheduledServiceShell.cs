using Cronos;

namespace WindowsServiceExample.Services
{
    public class BaseScheduledServiceShell<TService> where TService : IExcuteService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _semaphoreSlim;
        private bool _isRunning;

        public BaseScheduledServiceShell(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _semaphoreSlim = new(1);
        }

        /// <summary> Execute (如正在執行時再次呼叫則不執行並return false) </summary>
        public bool InvokeService()
        {
            if (!_semaphoreSlim.Wait(0))
                return false;
            _isRunning = true;

            _ = Task.Run(async () =>
            {
                try
                {
                    await ExecuteImpl();
                }
                finally
                {
                    _isRunning = false;
                    _semaphoreSlim.Release();
                }
            });
            return true;
        }

        public virtual bool IsScheduled(DateTimeOffset from)
        {
            // todo
            // CronExpression expression = CronExpression.Parse("*/5 * * * *");
            // DateTimeOffset? next = expression.GetNextOccurrence(from, TimeZoneInfo.Local);
            return true;
        }

        /// <summary> 執行 </summary>
        private Task ExecuteImpl()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<TService>();
            return service.Execute();
        }
    }
}
