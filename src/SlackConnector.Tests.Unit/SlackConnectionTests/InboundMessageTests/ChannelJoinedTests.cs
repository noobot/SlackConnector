using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    internal class given_bot_joins_channel : BaseTest<ChannelJoinedMessage>
    {
        private readonly string _hubId = "Woozah";
        private SlackChatHub _lastHub;

        protected override void Given()
        {
            base.Given();

            SUT.OnChatHubJoined += hub =>
            {
                _lastHub = hub;
                return Task.FromResult(false);
            };

            InboundMessage = new ChannelJoinedMessage
            {
                Channel = new Channel { Id = _hubId }
            };
        }

        [Test]
        public void then_should_raise_event_with_channel_information()
        {
            _lastHub.Id.ShouldEqual(_hubId);
            _lastHub.Type.ShouldEqual(SlackChatHubType.Channel);
        }

        [Test]
        public void then_should_add_channel_to_connected_hubs()
        {
            SUT.ConnectedHubs.ContainsKey(_hubId).ShouldBeTrue();
            SUT.ConnectedHubs[_hubId].ShouldEqual(_lastHub);
        }
    }

    internal class given_missing_channel_info_when_bot_joins_a_channel : BaseTest<ChannelJoinedMessage>
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

            InboundMessage = new ChannelJoinedMessage
            {
                Channel = null
            };
        }

        [Test]
        public void then_should_not_raise_evnet()
        {
            _lastHub.ShouldBeNull();
        }

        [Test]
        public void then_should_not_modify_connect_hubs()
        {
            SUT.ConnectedHubs.ShouldBeEmpty();
        }
    }
}