namespace WindowsServiceExample.Dtos
{
    public class JobStatusViewModel
    {
        public string JobIdentity { get; set; }
        public string JobName { get; set; }
        public string CronExpression { get; set; }
        public bool IsRunning { get; set; }
    }

}
