using System;
using System.IO;

namespace OwlSchedulerLibrary.OwlLogger
{
    public sealed class LogHandler
    {
        private static readonly Lazy<LogHandler> LazySingleton = new Lazy<LogHandler>(() => new LogHandler());

        public static LogHandler Instance => LazySingleton.Value;

        private const string LogDirectory = @"log";
        private const string LogMaster = @"log/log.txt";

        private LogHandler()
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }

                if (Directory.Exists(LogDirectory))
                {
                    if (!File.Exists(LogMaster))
                    {
                        File.Create(LogMaster);
                    }
                }
                
                LogMessage("Logger", "Init complete.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void LogMessage(string source, string messageToLog)
        {
            using (var writer = File.AppendText(LogMaster))
            {
                writer.WriteLine(DateTime.Now + ":" + source +": "+ messageToLog);
                writer.Close();
            }
        }   
    }
}