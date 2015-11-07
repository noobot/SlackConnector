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

            var slackConnector = new SlackConnector();
            slackConnector.OnConnectionStatusChanged += SlackConnectorOnConnectionStatusChanged;
            slackConnector.OnMessageReceived += SlackConnectorOnOnMessageReceived;

            // when
            slackConnector.Connect(config.Slack.ApiToken).Wait();

            // then
            Assert.That(slackConnector.IsConnected, Is.True);
        }

        private void SlackConnectorOnConnectionStatusChanged(bool isConnected)
        {

        }

        private Task SlackConnectorOnOnMessageReceived(SlackMessage message)
        {
            return new Task(() => { });
        }
    }
}