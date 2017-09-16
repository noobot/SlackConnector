using System;
using System.Linq;
using System.Threading.Tasks;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Integration
{
    public class JoinDmChannelTests : IntegrationTest
    {
        [Fact]
        public async Task should_join_channel()
        {
            // given
            if (string.IsNullOrEmpty(Config.Slack.TestUserName))
            {
                throw new InvalidConfiguration("TestUserName is missing from config");
            }
            
            var users = await SlackConnection.GetUsers();
            string userId = users.First(x => x.Name.Equals(Config.Slack.TestUserName, StringComparison.InvariantCultureIgnoreCase)).Id;

            // when
            SlackChatHub result = await SlackConnection.JoinDirectMessageChannel(userId);

            // then
            result.ShouldNotBeNull();

            var dmChannel = SlackConnection.ConnectedDM($"@{Config.Slack.TestUserName}");
            dmChannel.ShouldNotBeNull();
            await SlackConnection.Say(new BotMessage { ChatHub = dmChannel, Text = "Wuzzup - testing in da haus" });
        }
    }
}