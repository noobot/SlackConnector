using System.Threading.Tasks;
using SlackConnector.Models;
using Xunit;

namespace SlackConnector.Tests.Integration
{
    public class SayTests : IntegrationTest
    {
        [Fact]
        public async Task should_say_something_on_channel()
        {
            // given
            var message = new BotMessage
            {
                Text = "Test text for INT test",
                ChatHub = SlackConnection.ConnectedChannel(Config.Slack.TestChannel)
            };
            
            // when
            await SlackConnection.Say(message);

            // then
        }
    }
}