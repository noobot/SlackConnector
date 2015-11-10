using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.SlackConnectionTests.Setups;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class HubsTests
    {
        public class given_channels_are_archived : SpecsFor<SlackConnector>
        {
            private SlackHandshake Handshake { get; set; }
            private SlackConnectionFactoryStub SlackFactoryStub { get; set; }

            protected override void InitializeClassUnderTest()
            {
                SlackFactoryStub = new SlackConnectionFactoryStub();
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object, SlackFactoryStub);
            }

            protected override void Given()
            {
                Handshake = new SlackHandshake
                {
                    Ok = true,
                    Channels = new[]
                    {
                        new Channel
                        {
                            Id = "Id1",
                            Name = "Name1",
                            IsArchived = true,
                            IsMember = true //TODO: Need to do self things
                        },
                        new Channel
                        {
                            Id = "Id2",
                            Name = "Name2",
                            IsArchived = true,
                            IsMember = true //TODO: Need to do self things
                        },
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(Handshake);

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(Handshake.WebSocketUrl))
                    .Returns(GetMockFor<IWebSocketClient>().Object);
            }

            protected override void When()
            {
                SUT.Connect("something").Wait();
            }

            [Test]
            public void then_should_not_contain_channels()
            {
                SlackFactoryStub.Create_ConnectionInformation.SlackChatHubs.Count.ShouldEqual(0);
            }
        }
        
        public class given_groups_are_archived : SpecsFor<SlackConnector>
        {
            private SlackHandshake Handshake { get; set; }
            private SlackConnectionFactoryStub SlackFactoryStub { get; set; }

            protected override void InitializeClassUnderTest()
            {
                SlackFactoryStub = new SlackConnectionFactoryStub();
                SUT = new SlackConnector(GetMockFor<IConnectionFactory>().Object, SlackFactoryStub);
            }

            protected override void Given()
            {
                Handshake = new SlackHandshake
                {
                    Ok = true,
                    Groups = new[]
                    {
                        new Group
                        {
                            Id = "group-id",
                            Name = "group-name",
                            IsArchived = true,
                            //Members = new [] { SelfId } //TODO: Need to do self things
                        }
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(It.IsAny<string>()))
                    .ReturnsAsync(Handshake);

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateHandshakeClient())
                    .Returns(GetMockFor<IHandshakeClient>().Object);

                GetMockFor<IConnectionFactory>()
                    .Setup(x => x.CreateWebSocketClient(Handshake.WebSocketUrl))
                    .Returns(GetMockFor<IWebSocketClient>().Object);
            }

            protected override void When()
            {
                SUT.Connect("key").Wait();
            }

            [Test]
            public void then_should_not_contain_channels()
            {
                SlackFactoryStub.Create_ConnectionInformation.SlackChatHubs.Count.ShouldEqual(0);
            }
        }

        public class given_groups_that_are_not_archived_and_is_not_a_member_of_group : SlackConnectorIsSetup
        {
            private SlackHandshake _handshake;
            private const string SelfId = "abc123";
            
            protected override void Given()
            {
                base.Given();

                _handshake = new SlackHandshake
                {
                    Ok = true,
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

        public class given_instant_message_channel_when_user_id_doesnt_exists_in_cache : SlackConnectorIsSetup
        {
            private SlackHandshake _handshake;
            
            protected override void Given()
            {
                base.Given();

                _handshake = new SlackHandshake
                {
                    Ok = true,
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