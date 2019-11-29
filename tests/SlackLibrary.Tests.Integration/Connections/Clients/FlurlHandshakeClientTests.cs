using System.Threading.Tasks;
using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Clients.Handshake;
using SlackLibrary.Connections.Responses;
using SlackLibrary.Tests.Integration.Configuration;
using Xunit;
using Shouldly;

namespace SlackLibrary.Tests.Integration.Connections.Clients
{
    public class FlurlHandshakeClientTests
    {
        [Fact]
        public async Task should_perform_handshake_with_flurl()
        {
            // given
            var config = new ConfigReader().GetConfig();
            var client = new FlurlHandshakeClient(new ResponseVerifier());

            // when
            HandshakeResponse response = await client.FirmShake(config.Slack.ApiToken);

            // then
            response.ShouldNotBeNull();
            response.WebSocketUrl.ShouldNotBeEmpty();
        }
    }
}