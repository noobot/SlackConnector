using Moq;
using SlackConnector.BotHelpers;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Sockets;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.Setups
{
    internal abstract class SlackConnectorIsSetup : SpecsFor<SlackConnection>
    {
        protected override void InitializeClassUnderTest()
        {
            SUT = new SlackConnection(GetMockFor<IConnectionFactory>().Object, GetMockFor<IChatHubInterpreter>().Object, GetMockFor<IMentionDetector>().Object);
        }

        protected override void Given()
        {
            GetMockFor<IConnectionFactory>()
                .Setup(x => x.CreateHandshakeClient())
                .Returns(GetMockFor<IHandshakeClient>().Object);

            GetMockFor<IHandshakeClient>()
                .Setup(x => x.FirmShake(It.IsAny<string>()))
                .ReturnsAsync(new SlackHandshake { Ok = true });

            GetMockFor<IConnectionFactory>()
                .Setup(x => x.CreateWebSocketClient(It.IsAny<string>()))
                .Returns(GetMockFor<IWebSocketClient>().Object);
        }
    }
}