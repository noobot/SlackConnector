using System.Threading.Tasks;
using SlackConnector.Models;
using Xunit;

namespace SlackConnector.Tests.Integration
{
    public class TypingIndicatorTests : IntegrationTest
    {
        [Fact]
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