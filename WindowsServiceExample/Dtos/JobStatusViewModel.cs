namespace WindowsServiceExample.Dtos
{
    public class JobStatusViewModel
    {
        public required string JobIdentity { get; init; }
        public required string JobName { get; init; }
        public required string CronExpression { get; init; }
        public bool IsRunning { get; set; }
    }

}
