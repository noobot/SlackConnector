using System;
using System.Runtime.Serialization;

namespace SlackConnector.Exceptions
{
    public class AlreadyConnectedException : Exception
    {
        public AlreadyConnectedException()
        { }

        public AlreadyConnectedException(string message) : base(message)
        { }

        public AlreadyConnectedException(string message, Exception innerException) : base(message, innerException)
        { }

        protected AlreadyConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}