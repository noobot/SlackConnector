using SlackConnector.Connections.Handshaking.Models;

namespace SlackConnector.Tests.Unit.SlackConnectorTests.Connect
{
    internal static class Responses
    {
        public static SlackHandshake ValidHandshake()
        {
            return new SlackHandshake
            {
                Ok = true,
                Team = new Detail(),
                Self = new Detail(),
            };
        }
    }
}