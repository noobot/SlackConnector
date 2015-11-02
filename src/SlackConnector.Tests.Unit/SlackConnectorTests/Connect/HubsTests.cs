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
                SUT.Connect("something").Wait();
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
                SUT.ConnectedHubs[_handshake.Channels[0].Id].ShouldLookLike(expected);
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
                SUT.ConnectedHubs[_handshake.Channels[1].Id].ShouldLookLike(expected);
            }
        }

        public class given_groups_that_are_not_archived_and_is_a_member_of_group : SpecsFor<SlackConnector>
        {
            private SlackHandshake _handshake;
            private const string SelfId = "abc123";

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
                    Self = new Detail { Id = SelfId },
                    Groups = new[]
                    {
                        new Group
                        {
                            Id = "group-id",
                            Name = "group-name",
                            IsArchived = false,
                            Members = new [] { SelfId }
                        }
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(_handshake);
            }

            protected override void When()
            {
                SUT.Connect("key").Wait();
            }

            [Test]
            public void then_should_contain_single_channel()
            {
                SUT.ConnectedHubs.Count.ShouldEqual(1);
            }

            [Test]
            public void then_should_contain_channel()
            {
                var expected = new SlackChatHub
                {
                    Id = _handshake.Groups[0].Id,
                    Name = "#" + _handshake.Groups[0].Name,
                    Type = SlackChatHubType.Group,
                };
                SUT.ConnectedHubs[_handshake.Groups[0].Id].ShouldLookLike(expected);
            }
        }

        public class given_groups_that_are_not_archived_and_is_not_a_member_of_group : SpecsFor<SlackConnector>
        {
            private SlackHandshake _handshake;
            private const string SelfId = "abc123";

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
                    Self = new Detail { Id = SelfId },
                    Groups = new[]
                    {
                        new Group
                        {
                            Id = "group-id",
                            Name = "group-name",
                            IsArchived = false,
                            Members = new [] { "something-else" }
                        }
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(_handshake);
            }

            protected override void When()
            {
                SUT.Connect("key").Wait();
            }

            [Test]
            public void then_should_not_contain_any_channel()
            {
                SUT.ConnectedHubs.Count.ShouldEqual(0);
            }
        }

        public class given_instant_message_channel_when_user_id_doesnt_exists_in_cache : SpecsFor<SlackConnector>
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
                    Users = new []
                    {
                        new User
                        {
                            Id = "different-id-thingy",
                            Name = "different-namey-thingy"
                        }
                    },
                    Ims = new []
                    {
                        new Im
                        {
                            Id = "im-id",
                            User = "user-id-thingy"
                        }
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(_handshake);
            }

            protected override void When()
            {
                SUT.Connect("something").Wait();
            }

            [Test]
            public void then_should_contain_single_channel()
            {
                SUT.ConnectedHubs.Count.ShouldEqual(1);
            }

            [Test]
            public void then_should_contain_instant_message()
            {
                var expected = new SlackChatHub
                {
                    Id = _handshake.Ims[0].Id,
                    Name = "@" + _handshake.Ims[0].User,
                    Type = SlackChatHubType.DM,
                };
                SUT.ConnectedHubs[_handshake.Ims[0].Id].ShouldLookLike(expected);
            }
        }
    }
}