﻿using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WindowsServiceExample.Services.Example1;
using WindowsServiceExample.Services.Example2;

namespace WindowsServiceExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Host.UseWindowsService(option =>
            {
                option.ServiceName = "WindowsServiceExample";
            });
            builder.Services.AddControllers();
            builder.Services.AddSingleton<IJobFactory, JobFactory>();
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            builder.Services.AddHostedService<ExampleBackgroundService>();
            builder.Services.AddSingleton<Example1Service>();
            builder.Services.AddSingleton<Example2Service>();
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
