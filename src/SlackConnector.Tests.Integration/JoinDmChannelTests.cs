using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class JoinDmChannelTests : IntegrationTest
    {
        [Test]
        public async Task should_join_channel()
        {
            // given
            if (string.IsNullOrEmpty(Config.Slack.TestUserName))
            {
                Assert.Inconclusive("TestUserName is missing from config");
            }
            
            var users = await SlackConnection.GetUsers();
            string userId = users.First(x => x.Name.Equals(Config.Slack.TestUserName, StringComparison.InvariantCultureIgnoreCase)).Id;

            // when
            SlackChatHub result = await SlackConnection.JoinDirectMessageChannel(userId);

            // then
            Assert.That(result, Is.Not.Null);

            var dmChannel = SlackConnection.ConnectedDM($"@{Config.Slack.TestUserName}");
            Assert.That(dmChannel, Is.Not.Null);
            await SlackConnection.Say(new BotMessage { ChatHub = dmChannel, Text = "Wuzzup - testing in da haus" });
        }
    }
}