using Moq;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Models;
using SlackConnector.Tests.Unit.SlackConnectionTests.Setups;

namespace SlackConnector.Tests.Unit.SlackConnectorTests
{
    public static class TeamAndSelfTests
    {
        public class given_api_key_when_connecting_to_slack : SlackConnectorIsSetup
        {
            private readonly string SlackKey = "000111ooo";
            private SlackHandshake _handshake;
            
            protected override void Given()
            {
                base.Given();
                _handshake = new SlackHandshake
                {
                    Ok = true,
                    Team = new Detail
                    {
                        Id = "team-id-value",
                        Name = "team-name-value"
                    },
                    Self = new Detail
                    {
                        Id = "self-id-value",
                        Name = "self-name-value"
                    }
                };

                GetMockFor<IHandshakeClient>()
                    .Setup(x => x.FirmShake(SlackKey))
                    .ReturnsAsync(_handshake);
            }

            protected override void When()
            {
                SUT.Connect(SlackKey).Wait();
            }

            [Test]
            public void then_should_set_slack_key()
            {
                SUT.SlackKey.ShouldEqual(SlackKey);
            }

            [Test]
            public void then_should_populate_team_details()
            {
                SUT.TeamId.ShouldEqual(_handshake.Team.Id);
                SUT.TeamName.ShouldEqual(_handshake.Team.Name);
            }

            [Test]
            public void then_should_populate_self_user_details()
            {
                SUT.UserId.ShouldEqual(_handshake.Self.Id);
                SUT.UserName.ShouldEqual(_handshake.Self.Name);
            }
        }
    }
}