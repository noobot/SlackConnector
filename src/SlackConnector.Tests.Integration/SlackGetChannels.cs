using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    public class SlackGetChannels
    {
        [TestFixture]
        public class SlackConnectorTests
        {
            [Test]
            public async void should_connect_and_get_channels()
            {
                // given
                var config = new ConfigReader().GetConfig();
                var slackConnector = new SlackConnector();

                // when
                var connection = await slackConnector.Connect(config.Slack.ApiToken);
                var channels = await connection.GetChannels();

                // then
                Assert.That(channels.Any(), Is.True);
            }
        }
    }
}