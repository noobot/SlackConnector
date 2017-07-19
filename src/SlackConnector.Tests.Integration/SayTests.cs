using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class SayTests : IntegrationTest
    {
        [Test]
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