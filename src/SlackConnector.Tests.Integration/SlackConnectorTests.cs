using System;
using System.Threading;
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

            // when
            ISlackConnector slackConnector = new SlackConnector();
            slackConnector.OnConnectionStatusChanged += SlackConnectorOnConnectionStatusChanged;
            slackConnector.OnMessageReceived += SlackConnectorOnOnMessageReceived;
            slackConnector.Connect(config.Slack.ApiToken);

            // then
            DateTime startTime = DateTime.Now;
            while (!slackConnector.IsConnected && (DateTime.Now - startTime) < TimeSpan.FromSeconds(30))
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            Assert.That(slackConnector.IsConnected, Is.True);
        }

        private void SlackConnectorOnConnectionStatusChanged(bool isConnected)
        {

        }

        private Task SlackConnectorOnOnMessageReceived(ResponseContext message)
        {
            return new Task(() => { });
        }
    }
}