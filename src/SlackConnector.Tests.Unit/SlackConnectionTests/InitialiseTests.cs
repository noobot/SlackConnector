using NUnit.Framework;
using Should;
using SlackConnector.Models;
using SpecsFor;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public static class InitialiseTests
    {
        internal class given_valid_connection_info : SpecsFor<SlackConnection>
        {
            private ConnectionInformation Info { get; set; }

            protected override void Given()
            {
                Info = new ConnectionInformation
                {
                    Self = new ContactDetails { Id = "self-id" },
                    Team = new ContactDetails { Id = "team-id" },
                };
            }

            protected override void When()
            {
                SUT.Initialise(Info);
            }

            [Test]
            public void then_should_populate_self()
            {
                SUT.Self.ShouldEqual(Info.Self);
            }

            [Test]
            public void then_should_populate_team()
            {
                SUT.Team.ShouldEqual(Info.Team);
            }
        }
    }
}