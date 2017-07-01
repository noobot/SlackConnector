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
            var response = await connection.Say(message);

            // then
            Assert.That(string.CompareOrdinal(response.Message.Text, message.Text) == 0, Is.True);
        }

        [Test]
        public async Task should_say_something_on_channel_then_delete_it()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            var connection = await slackConnector.Connect(config.Slack.ApiToken);
            var message = new BotMessage
            {
                Text = "Test delete this text for INT test",
                ChatHub = connection.ConnectedChannel(config.Slack.TestChannel)
            };

            // when
            var messageResponse = await connection.Say(message);
            // Wait for 2 seconds for dramatic pause
            await Task.Delay(2000);
            // Now delete
            var deleteResponse = await connection.DeleteMessage(messageResponse.Channel, messageResponse.Message.TimeStamp);

            // then
            Assert.That(deleteResponse.Channel == message.ChatHub.Id, Is.True);
        }
    }
}