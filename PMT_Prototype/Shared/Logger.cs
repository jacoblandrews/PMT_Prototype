using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PMT_Prototype.Shared
{
    public sealed class Logger
    {
        private static string? _logFileName;
        private static string? LogFileName { get => _logFileName; set { _logFileName = value; } }

        private static readonly SemaphoreSlim semaphore = new(1);
        private static readonly Lazy<Logger> logger = new(() => new Logger());

        public static Logger Instance => logger.Value;

        public static async Task Log(string message, LogLevel level)
        {
            await semaphore.WaitAsync(500);

            try
            {
                //if the log file name isn't set or is out of date, update it
                if (string.IsNullOrWhiteSpace(LogFileName) || LogFileName != $"{DateTimeOffset.UtcNow.Day}_{DateTimeOffset.UtcNow.Month}_{DateTimeOffset.UtcNow.Year}.txt")
                {
                    LogFileName = $"{DateTimeOffset.UtcNow.Day}_{DateTimeOffset.UtcNow.Month}_{DateTimeOffset.UtcNow.Year}.txt";
                }

                //create a directory here
                Directory.CreateDirectory($"./logs/{LogFileName}");

                message = $"{DateTimeOffset.UtcNow}: LOG LEVEL: {level} - Message: {Environment.NewLine} {message}";

                await File.AppendAllTextAsync(LogFileName, message);

            }
            finally
            {
                semaphore.Release();
            }

        }

        public static async Task Log(Exception exception, LogLevel level)
        {
            var formattedMessage = $@"
            |Message: {exception.Message}|
            |Source: {exception.Source}|
            |Stack Trace: {exception.StackTrace}|
            ";

            await Log(formattedMessage, level);
        }

        public enum LogLevel
        {
            Information,
            Warning,
            Error,
            Critical
        }



    }
}
