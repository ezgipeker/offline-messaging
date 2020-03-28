using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;

namespace OfflineMessaging.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "OfflineMessaging.Api")
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .WriteTo.Debug()
                    .WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                    .WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(
                            new Uri(hostingContext.Configuration["Serilog:ElasticsearchEndpoint"]))
                        {
                            AutoRegisterTemplate = true,
                            ConnectionTimeout = TimeSpan.FromMilliseconds(5000),
                            MinimumLogEventLevel = hostingContext.Configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel"),
                            TemplateName = "serilog-events-template",
                            IndexFormat = "api-log-{0:yyyy.MM.dd}",
                            ModifyConnectionSettings = x =>
                                x.BasicAuthentication(hostingContext.Configuration["Serilog:ElasticsearchUserName"],
                                    hostingContext.Configuration["Serilog:ElasticsearchPassword"])
                        })
                    .MinimumLevel.Is(hostingContext.Configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel")));

    }
}
