using System;

namespace SlackConnector.Logging
{
    public class Logger : ILogger
    {
        public void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}