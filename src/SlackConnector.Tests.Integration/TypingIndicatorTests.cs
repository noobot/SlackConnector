using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class TypingIndicatorTests
    {
        [Test]
        public async Task should_send_typing_indicator()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            var connection = await slackConnector.Connect(config.Slack.ApiToken);
            SlackChatHub channel = connection.ConnectedChannel(config.Slack.TestChannel);

            // when
            await connection.IndicateTyping(channel);

            // then
        }
    }
}