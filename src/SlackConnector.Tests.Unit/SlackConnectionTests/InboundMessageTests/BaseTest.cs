using System.Threading.Tasks;
using SlackConnector.Connections.Sockets;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal abstract class BaseTest<TMessage> : SpecsFor<SlackConnection> where TMessage : InboundMessage
    {
        protected TMessage InboundMessage { get; set; }
        protected bool MessageRaised { get; set; }
        protected SlackMessage Result { get; set; }
        protected ConnectionInformation ConnectionInfo { get; set; }

        protected override void Given()
        {
            SUT.OnMessageReceived += async message =>
            {
                Result = message;
                MessageRaised = true;
                await Task.CompletedTask;
            };

            ConnectionInfo = new ConnectionInformation { WebSocket = GetMockFor<IWebSocketClient>().Object };
        }

        protected override void When()
        {
            SUT.Initialise(ConnectionInfo);
            GetMockFor<IWebSocketClient>()
                .Raise(x => x.OnMessage += null, null, InboundMessage);
        }
    }
}