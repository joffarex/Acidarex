﻿using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace AcidarX.Core
{
    public static class AXLoggerExtensions
    {
        [Conditional("DEBUG")]
        public static void Assert<T>(this ILogger<T> logger, bool condition, string message)
        {
            if (!condition)
            {
                logger.LogError(string.Format("Assertion Failed: {0}", message));
                Debugger.Break();
            }
        }
    }

    public static class AXLogger
    {
        private const string OutputTemplate =
            "[{Timestamp:HH:mm}] [{SourceContext}] [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}";

        private static readonly Logger SerilogLogger = new LoggerConfiguration().MinimumLevel.Verbose().MinimumLevel
            .Override("Microsoft", LogEventLevel.Information)
            .Enrich.With(new ThreadEnricher())
            .WriteTo.Console(outputTemplate: OutputTemplate)
            .CreateLogger();

        public static ILogger<T> CreateLogger<T>() where T : class =>
            new LoggerFactory().AddSerilog(SerilogLogger).CreateLogger<T>();
    }

    public class ThreadEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty("TheadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}