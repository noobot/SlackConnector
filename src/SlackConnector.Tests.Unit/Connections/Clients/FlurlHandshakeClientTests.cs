using System.Threading.Tasks;
using ExpectedObjects;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Tests.Unit.Connections.Clients
{
    [TestFixture]
    public class FlurlHandshakeClientTests
    {
        private HttpTest _httpTest;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
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
            _httpTest.RespondWithJson(new HandshakeResponse());
            var client = new FlurlHandshakeClient();

            // when
            await client.FirmShake(slackKey);

            // then
            _httpTest
                .ShouldHaveCalled(ClientConstants.HANDSHAKE_PATH.AppendPathSegment(FlurlHandshakeClient.HANDSHAKE_PATH))
                .WithQueryParamValue("token", slackKey)
                .Times(1);
        }

        [Test]
        public async Task should_return_expected_data()
        {
            // given
            var expectedResponse = new HandshakeResponse
            {
                Ok = true,
                WebSocketUrl = "some-url"
            };
            _httpTest.RespondWithJson(expectedResponse);
            var client = new FlurlHandshakeClient();

            // when
            var result = await client.FirmShake(It.IsAny<string>());

            // then
            result.ToExpectedObject().ShouldEqual(expectedResponse);
        }
    }
}