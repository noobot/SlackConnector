using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class SlackConnectorTests
    {
        [Test]
        public void should_connect_and_stuff()
        {
            // given
            var config = new ConfigReader().GetConfig();
            if (config.IsConfigured == false) { Assert.Ignore(); }

            var slackConnector = new SlackConnector();

            // when
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;
            connection.OnDisconnect += SlackConnector_OnDisconnect;
            connection.OnMessageReceived += SlackConnectorOnOnMessageReceived;

            // then
            Assert.That(connection.IsConnected, Is.True);
        }

        private void SlackConnector_OnDisconnect()
        {

        }

        private Task SlackConnectorOnOnMessageReceived(SlackMessage message)
        {
            return new Task(() => { });
        }
    }
}