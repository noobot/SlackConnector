using System.Threading.Tasks;
using ExpectedObjects;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.TestExtensions;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    [TestFixture]
    public class FlurlHandshakeClientTests
    {
        private HttpTest _httpTest;
        private Mock<IResponseVerifier> _responseVerifierMock;
        private FlurlHandshakeClient _handshakeClient;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
            _responseVerifierMock = new Mock<IResponseVerifier>();
            _handshakeClient = new FlurlHandshakeClient(_responseVerifierMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _httpTest.Dispose();
        }

        [Test]
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