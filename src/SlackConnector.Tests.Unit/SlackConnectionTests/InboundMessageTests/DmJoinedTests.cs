using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal class given_new_dm_channel : BaseTest<DmChannelJoinedMessage>
    {
        private SlackChatHub _lastHub;

        protected override void Given()
        {
            base.Given();

            SUT.OnChatHubJoined += hub =>
            {
                _lastHub = hub;
                return Task.FromResult(false);
            };

            InboundMessage = new DmChannelJoinedMessage
            {
                Channel = new Im
                {
                    User = "test-user",
                    Id = "channel-id",
                    IsIm = true,
                    IsOpen = true
                }
            };
        }

        [Test]
        public void then_should_raised_event_with_expected_channel_information()
        {
            var expectedChatHub = new SlackChatHub
            {
                Id = "channel-id",
                Name = "@test-user",
                Type = SlackChatHubType.DM
            };
            _lastHub.ShouldLookLike(expectedChatHub);
        }

        [Test]
        public void then_should_add_channel_to_connected_hubs()
        {
            SUT.ConnectedHubs.ContainsKey("channel-id").ShouldBeTrue();
            SUT.ConnectedHubs["channel-id"].ShouldEqual(_lastHub);
        }
    }
}