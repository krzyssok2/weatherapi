using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System;

namespace WeatherForecastAPI.Logging
{
    internal static class LoggerSetup
    {
        public static void Configure(HostBuilderContext hostingContext)
        {
            var configuration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
                .WriteTo.File(path: "logs\\log-.txt",
                    LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    buffered: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(7))
                .WriteTo.MSSqlServer(
                    hostingContext.Configuration.GetConnectionString("MyWeatherAPIDatabase"),
                    new SinkOptions()
                    {
                        AutoCreateSqlTable = true,
                        BatchPeriod = TimeSpan.FromSeconds(7),
                        TableName = "Logs"
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information
                );

            Log.Logger = configuration.CreateLogger();
        }
    }
}