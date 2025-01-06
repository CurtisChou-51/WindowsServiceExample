
namespace WindowsServiceExample.Services
{
    public class Example1BgShellService : IBgShellService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _semaphoreSlim;

        public Example1BgShellService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _semaphoreSlim = new(1);
        }

        /// <summary> Execute (如正在執行時再次呼叫則不執行並return false) </summary>
        public bool Execute()
        {
            if (!_semaphoreSlim.Wait(0))
                return false;

            _ = Task.Run(async () =>
            {
                try
                {
                    await ExecuteImpl();
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
            return true;
        }

        /// <summary> 執行 </summary>
        private Task ExecuteImpl()
        {

            return Task.CompletedTask;
        }
    }
}
