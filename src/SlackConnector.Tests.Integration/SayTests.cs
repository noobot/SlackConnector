using System;
using System.Linq;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class SayTests
    {
        [Test]
        public void should_say_something_on_channel()
        {
            // given
            var config = new ConfigReader().GetConfig();

            ISlackConnector slackConnector = new SlackConnector();
            slackConnector.Connect(config.Slack.ApiToken).Wait();

            var message = new BotMessage
            {
                Text = "Test text for INT test",
                ChatHub = slackConnector.ConnectedChannels.First(x => x.Name.Equals("#general", StringComparison.InvariantCultureIgnoreCase))
            };

            // when
            slackConnector.Say(message).Wait();

            // then

        }
    }
}