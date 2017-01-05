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
        private ISlackConnection _slackConnection; 

        [Test]
        public void should_connect_and_stuff()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();

            // when
            _slackConnection = slackConnector.Connect(config.Slack.ApiToken).Result;
            _slackConnection.OnDisconnect += SlackConnector_OnDisconnect;
            _slackConnection.OnMessageReceived += SlackConnectorOnMessageReceived;

            // then
            Assert.That(_slackConnection.IsConnected, Is.True);
        }

        private void SlackConnector_OnDisconnect()
        {

        }

        private Task SlackConnectorOnMessageReceived(SlackMessage message)
        {
            return new Task(() => { });
        }
    }
}