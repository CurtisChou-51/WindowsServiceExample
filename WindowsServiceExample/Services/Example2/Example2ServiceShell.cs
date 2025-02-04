namespace WindowsServiceExample.Services.Example2
{
    public class Example2ServiceShell : BaseScheduledServiceShell<IExample2Service>, IScheduledServiceShell
    {
        public Example2ServiceShell(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool IsScheduled(DateTimeOffset from)
        {
            // todo
            // CronExpression expression = CronExpression.Parse("*/5 * * * *");
            // DateTimeOffset? next = expression.GetNextOccurrence(from, TimeZoneInfo.Local);
            return true;
        }
    }
}
