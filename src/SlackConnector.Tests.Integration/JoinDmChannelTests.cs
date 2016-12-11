using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class JoinDmChannelTests
    {
        [Test]
        public void should_join_channel()
        {
            // given
            var config = new ConfigReader().GetConfig();
            if (string.IsNullOrEmpty(config.Slack.TestUserId))
            {
                Assert.Inconclusive("TestUserId is missing from config");
            }

            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;

            // when
            SlackChatHub result = connection.JoinDirectMessageChannel(config.Slack.TestUserId).Result;

            // then
            Assert.That(result, Is.Not.Null);

            List<SlackChatHub> dms = connection.ConnectedDMs().ToList();
            Assert.That(dms.Count, Is.GreaterThan(1));
        }
    }
}