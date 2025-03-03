namespace WindowsServiceExample.Dtos
{
    public class JobScheduleDto
    {
        public string JobIdentity { get; set; }
        public string JobName { get; set; }
        public Type JobType { get; set; }
        public string CronExpression { get; set; }
    }

}
