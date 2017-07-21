using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class TypingIndicatorTests : IntegrationTest
    {
        [Test]
        public async Task should_send_typing_indicator()
        {
            // given
            SlackChatHub channel = SlackConnection.ConnectedChannel(Config.Slack.TestChannel);

            // when
            await SlackConnection.IndicateTyping(channel);

            // then
        }
    }
}