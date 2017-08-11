using System;
using System.Threading.Tasks;
using ExpectedObjects;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    public class FlurlHandshakeClientTests : IDisposable
    {
        private readonly HttpTest _httpTest;
        private readonly Mock<IResponseVerifier> _responseVerifierMock;
        private readonly FlurlHandshakeClient _handshakeClient;
        
        public FlurlHandshakeClientTests()
        {
            _httpTest = new HttpTest();
            _responseVerifierMock = new Mock<IResponseVerifier>();
            _handshakeClient = new FlurlHandshakeClient(_responseVerifierMock.Object);
        }
        
        public void Dispose()
        {
            _httpTest.Dispose();
        }

        [Fact]
        public async Task should_call_expected_url_with_given_slack_key()
        {
            // given
            const string slackKey = "I-is-da-key-yeah";

            var expectedResponse = new HandshakeResponse
            {
                Ok = true,
                WebSocketUrl = "some-url"
            };
            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _handshakeClient.FirmShake(slackKey);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)));
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlHandshakeClient.HANDSHAKE_PATH))
                .WithQueryParamValue("token", slackKey)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse);
        }
    }
}