using System;

namespace Server.Logging
{
    class ConsoleLogger : ILogger
    {
        public void Log(string actionType, string actionInfo)
        {
            var date = DateTime.Now;
            Console.WriteLine($"[{date.ToShortDateString()}|{date.ToShortTimeString()}][{actionType}]: {actionInfo}");
        }
    }
}
