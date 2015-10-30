using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectorTests.Connect
{
    public static class UsersTests
    {
        public class given_user_cache_when_connecting_to_slack : SpecsFor<SlackConnector>
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
                            Id = "Id1",
                            Name = "Name1"
                        },
                        new User
                        {
                            Id = "Id2",
                            Name = "Name2"
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
            public void then_should_contain_2_users()
            {
                SUT.UserNameCache.Count.ShouldEqual(2);
            }

            [Test]
            public void then_should_contain_user_1()
            {
                SUT.UserNameCache["Id1"].ShouldEqual("Name1");
            }

            [Test]
            public void then_should_contain_user_2()
            {
                SUT.UserNameCache["Id2"].ShouldEqual("Name2");
            }
        }
    }
}