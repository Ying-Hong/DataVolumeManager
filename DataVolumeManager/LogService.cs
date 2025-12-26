using System;
using System.IO;

namespace DataVolumeManager
{
    public static class LogService
    {
        private static readonly object _lock = new object();

        public static void Write(string msg)
        {
            lock (_lock)
            {
                Directory.CreateDirectory("logs");
                string file = Path.Combine("logs", $"cleaner_{DateTime.Now:yyyyMMdd}.log");
                File.AppendAllText(file, $"{DateTime.Now:HH:mm:ss} {msg}\r\n");
            }
        }
    }
}
