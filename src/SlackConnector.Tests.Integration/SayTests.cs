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

            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;
            var message = new BotMessage
            {
                Text = "Test text for INT test",
                ChatHub = connection.ConnectedChannel(config.Slack.TestChannel)
            };
            
            // when
            connection.Say(message).Wait();

            // then
        }
    }
}