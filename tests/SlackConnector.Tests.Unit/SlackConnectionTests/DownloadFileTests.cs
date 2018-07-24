using System;
using System.Threading.Tasks;
using AutoFixture;
using Flurl.Http.Testing;
using Moq;
using Shouldly;
using SlackConnector.Connections.Sockets;
using SlackConnector.Models;
using Xunit;

namespace SlackConnector.Tests.Unit.SlackConnectionTests
{
    public class DownloadFileTests
    {
        [Theory, AutoMoqData]
        private async Task should_return_stream(
            Mock<IWebSocketClient> webSocket,
            SlackConnection slackConnection,
            string slackKey,
            Fixture fixture)
        {
            using (var httpTest = new HttpTest())
            {
                // given
                var downloadUri = new Uri($"https://files.slack.com/{fixture.Create<string>()}");

                var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
                await slackConnection.Initialise(connectionInfo);

                httpTest
                    .RespondWithJson(new { fakeObject = true });

                // when
                var result = await slackConnection.DownloadFile(downloadUri);

                // then
                result.ShouldNotBeNull();
                httpTest
                    .ShouldHaveCalled(downloadUri.AbsoluteUri)
                    .WithOAuthBearerToken(slackKey);
            }
        }

        [Theory, AutoMoqData]
        private async Task throws_exception_if_uri_isnt_slack(
            Mock<IWebSocketClient> webSocket,
            SlackConnection slackConnection,
            string slackKey,
            Fixture fixture)
        {
            // given
            var downloadUri = new Uri($"https://something.com/{fixture.Create<string>()}");

            var connectionInfo = new ConnectionInformation { WebSocket = webSocket.Object, SlackKey = slackKey };
            await slackConnection.Initialise(connectionInfo);

            // when
            var exception = Should.Throw<ArgumentException>(async () => await slackConnection.DownloadFile(downloadUri));

            // then
            exception.ShouldNotBeNull();
            exception.Message.ShouldContain("Invalid uri");
        }
    }
}