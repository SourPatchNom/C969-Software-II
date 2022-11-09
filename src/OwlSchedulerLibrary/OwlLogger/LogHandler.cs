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
            
        }

        public static bool Initialize()
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                    File.Create(LogMaster);
                    return true;
                }
                
                if (File.Exists(LogMaster)) return false;
                File.Create(LogMaster);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public static void LogMessage(string source, string messageToLog)
        {
            try
            {
                

                using (var writer = File.AppendText(LogMaster))
                {
                    writer.WriteLine(DateTime.Now + ":" + source + ": " + messageToLog);
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }   
    }
}