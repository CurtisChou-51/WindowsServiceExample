
namespace WindowsServiceExample.Services
{
    public interface IScheduledServiceShell
    {
        bool InvokeService();

        bool IsScheduled(DateTimeOffset from);
    }
}