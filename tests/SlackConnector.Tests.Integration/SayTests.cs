using System;
using System.Threading.Tasks;
using SlackConnector.Models;
using Xunit;
using Xunit.Abstractions;

namespace SlackConnector.Tests.Integration
{
    public class SayTests : IntegrationTest
    {
        private readonly ITestOutputHelper _output;

        public SayTests(ITestOutputHelper output)
        {
            _output = output;
        }

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

            SlackConnection.OnMessageReceived += slackMessage =>
            {
                _output.WriteLine(slackMessage.Text);
                return Task.CompletedTask;
            };

            // then
            //await Task.Delay(TimeSpan.FromMinutes(2));
        }
    }
}