using WindowsServiceExample.Services;
using WindowsServiceExample.Services.Example1;
using WindowsServiceExample.Services.Example2;

namespace WindowsServiceExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseWindowsService(option =>
            {
                option.ServiceName = "WindowsServiceExample";
            });
            builder.Services.AddControllers();
            builder.Services.AddHostedService<ExampleBackgroundService>();
            builder.Services.AddSingleton<IScheduledServiceShell, Example1ServiceShell>();
            builder.Services.AddSingleton<IScheduledServiceShell, Example2ServiceShell>();
            builder.Services.AddScoped<IExample1Service, Example1Service>();
            builder.Services.AddScoped<IExample2Service, Example2Service>();
            builder.Services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddFilter("System.Net.Http", LogLevel.Warning);
            });

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
