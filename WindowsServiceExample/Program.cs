using WindowsServiceExample.Services;

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
            builder.Services.AddSingleton<IBgShellService, Example1ShellService>();
            builder.Services.AddSingleton<IBgShellService, Example2ShellService>();

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
