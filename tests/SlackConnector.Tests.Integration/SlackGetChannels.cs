using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Integration
{
    public class SlackGetChannels : IntegrationTest
    {
        [Fact]
        public async Task should_connect_and_get_channels()
        {
            // given

            // when
            var channels = await SlackConnection.GetChannels();

            // then
            channels.Any().ShouldBeTrue();
        }

        [Fact]
        public async Task should_connect_and_get_users()
        {
            // given

            // when
            var users = await SlackConnection.GetUsers();

            // then
           users.Any(u => u.Online == true).ShouldBeTrue();
        }
    }
}