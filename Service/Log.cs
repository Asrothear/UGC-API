using NLog;
using NLog.Targets;
using System;

namespace UGC_API.Service
{
    public class Log
    {
        internal static void Configure()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $@"Logs\{GetTime.DateNow().ToString("yyyy-MM-dd")}.log" };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

        internal static void UpdateConfig()
        {
            var configuration = LogManager.Configuration;
            var fileTarget = configuration.FindTargetByName<FileTarget>("logfile");
            var fnam = fileTarget.FileName;
            fileTarget.FileName = $@"Logs\{ GetTime.DateNow().ToString("yyyy-MM-dd")}.log";
            LogManager.Configuration = configuration; //apply
        }
    }
}
