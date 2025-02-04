namespace WindowsServiceExample.Services.Example1
{
    public class Example1ServiceShell : BaseScheduledServiceShell<IExample1Service>, IScheduledServiceShell
    {
        public Example1ServiceShell(IServiceProvider serviceProvider) : base(serviceProvider)
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
