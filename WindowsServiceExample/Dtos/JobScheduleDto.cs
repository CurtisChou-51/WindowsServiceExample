namespace WindowsServiceExample.Dtos
{
    public class JobScheduleDto
    {
        public JobScheduleDto(string jobIdentity, string jobName, Type jobType, string cronExpression)
        {
            JobIdentity = jobIdentity;
            JobName = jobName;
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public string JobIdentity { get; set; }
        public string JobName { get; set; }
        public Type JobType { get; set; }
        public string CronExpression { get; set; }
    }

}
