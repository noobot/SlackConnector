using System.Threading.Tasks;
using SlackLibrary.Models;
using Xunit;

namespace SlackLibrary.Tests.Integration
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