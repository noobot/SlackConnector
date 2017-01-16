using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class SayTests
    {
        [Test]
        public async Task should_say_something_on_channel()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            var connection = await slackConnector.Connect(config.Slack.ApiToken);
            var message = new BotMessage
            {
                Text = "Test text for INT test",
                ChatHub = connection.ConnectedChannel(config.Slack.TestChannel)
            };
            
            // when
            await connection.Say(message);

            // then
        }
    }
}