namespace WindowsServiceExample.Services
{
    public class Example2ServiceShell : BaseScheduledServiceShell<IExample2Service>, IScheduledServiceShell
    {
        public Example2ServiceShell(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
