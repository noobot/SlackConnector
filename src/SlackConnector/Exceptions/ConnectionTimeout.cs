using System;

namespace SlackConnector.Exceptions
{
    public class ConnectionTimeout : Exception
    {
        public ConnectionTimeout(string message)
            : base(message)
        { }
    }
}