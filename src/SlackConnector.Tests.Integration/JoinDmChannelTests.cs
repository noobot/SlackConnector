using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class JoinDmChannelTests
    {
        [Test]
        public async Task should_join_channel()
        {
            // given
            var config = new ConfigReader().GetConfig();
            if (string.IsNullOrEmpty(config.Slack.TestUserName))
            {
                Assert.Inconclusive("TestUserName is missing from config");
            }

            var slackConnector = new SlackConnector();
            var connection = await slackConnector.Connect(config.Slack.ApiToken);
            var users = await connection.GetUsers();
            string userId = users.First(x => x.Name.Equals(config.Slack.TestUserName, StringComparison.InvariantCultureIgnoreCase)).Id;

            // when
            SlackChatHub result = await connection.JoinDirectMessageChannel(userId);

            // then
            Assert.That(result, Is.Not.Null);

            var dmChannel = connection.ConnectedDM($"@{config.Slack.TestUserName}");
            Assert.That(dmChannel, Is.Not.Null);
            await connection.Say(new BotMessage{ChatHub = dmChannel, Text = "Wuzzup - testing in da haus"});
        }
    }
}