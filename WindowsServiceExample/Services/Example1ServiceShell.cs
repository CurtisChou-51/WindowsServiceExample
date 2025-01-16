namespace WindowsServiceExample.Services
{
    public class Example1ServiceShell : BaseScheduledServiceShell<IExample1Service>, IScheduledServiceShell
    {
        public Example1ServiceShell(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
