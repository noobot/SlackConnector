using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SlackConnector.Tests.Integration
{
    public class SlackGetChannels
    {
        [TestFixture]
        public class SlackConnectorTests : IntegrationTest
        {
            [Test]
            public async Task should_connect_and_get_channels()
            {
                // given

                // when
                var channels = await SlackConnection.GetChannels();

                // then
                Assert.That(channels.Any(), Is.True);
            }

            [Test]
            public async Task should_connect_and_get_users()
            {
                // given

                // when
                var users = await SlackConnection.GetUsers();

                // then
                Assert.That(users.Any(u => u.Online == true), Is.True);
            }
        }
    }
}