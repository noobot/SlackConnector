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
            slackConnector.ConnectionStatusChanged += SlackConnector_ConnectionStatusChanged;

            slackConnector.MessageReceived+= SlackConnectorOnMessageReceived;

            slackConnector.Connect(config.Slack.ApiToken);

            // then
            Thread.Sleep(TimeSpan.FromSeconds(30));
        }

        private void SlackConnector_ConnectionStatusChanged(bool isConnected)
        {

        }

        private Task SlackConnectorOnMessageReceived(ResponseContext message)
        {
            return new Task(() => { });
        }
    }
}