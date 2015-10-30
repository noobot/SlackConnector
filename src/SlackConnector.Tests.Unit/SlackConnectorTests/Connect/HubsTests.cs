using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Models;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectorTests.Connect
{
    public static class HubsTests
    {
        public class given_channels_that_are_not_archived_and_are_a_member_of : SpecsFor<SlackConnector>
        {
            private SlackHandshake _handshake;

            protected override void InitializeClassUnderTest()
            {
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object);
            }

            protected override void Given()
            {
                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                _handshake = new SlackHandshake
                {
                    Channels = new[]
                    {
                        new Channel
                        {
                            Id = "Id1",
                            Name = "Name1",
                            IsArchived = false,
                            IsMember = true
                        },
                        new Channel
                        {
                            Id = "Id2",
                            Name = "Name2",
                            IsArchived = false,
                            IsMember = true
                        },
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(_handshake);
            }

            protected override void When()
            {
                SUT.Connect("").Wait();
            }

            [Test]
            public void then_should_contain_2_channels()
            {
                SUT.ConnectedHubs.Count.ShouldEqual(2);
            }

            [Test]
            public void then_should_contain_channel_1()
            {
                var expected = new SlackChatHub
                {
                    Id = _handshake.Channels[0].Id,
                    Name = "#" + _handshake.Channels[0].Name,
                    Type = SlackChatHubType.Channel,
                };
                SUT.ConnectedHubs["Id1"].ShouldLookLike(expected);
            }

            [Test]
            public void then_should_contain_channel_2()
            {
                var expected = new SlackChatHub
                {
                    Id = _handshake.Channels[1].Id,
                    Name = "#" + _handshake.Channels[1].Name,
                    Type = SlackChatHubType.Channel,
                };
                SUT.ConnectedHubs["Id2"].ShouldLookLike(expected);
            }
        }
    }
}