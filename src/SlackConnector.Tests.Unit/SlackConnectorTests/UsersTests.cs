using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Models;
using SlackConnector.Tests.Unit.SlackConnectionTests.Setups;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class UsersTests
    {
        public class given_user_cache_when_connecting_to_slack : SlackConnectorIsSetup
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
                SUT.Connect("key").Wait();
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