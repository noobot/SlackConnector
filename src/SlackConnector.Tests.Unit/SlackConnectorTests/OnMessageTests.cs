using SlackConnector.Sockets;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class OnMessageTests
    {
        public class given_standard_message_when_message_is_raised : SpecsFor<SlackConnector>
        {
            private WebSocketStub _webSocket = new WebSocketStub();

            protected override void Given()
            {
                //GetMockFor<IWebSocketFactory>()
                //    .Setup(x => x.Create())
                //    .
            }
        }
    }
}