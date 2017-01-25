using System.Threading.Tasks;
using ExpectedObjects;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.TestExtensions;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    [TestFixture]
    public class FlurlChatClientTests
    {
        private HttpTest _httpTest;
        private Mock<IResponseVerifier> _responseVerifierMock;
        private FlurlChatClient _chatClient;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
            _responseVerifierMock = new Mock<IResponseVerifier>();
            _chatClient = new FlurlChatClient(_responseVerifierMock.Object);
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
            const string slackKey = "something-that-looks-like-a-slack-key";
            const string channel = "channel-name";
            const string text = "some-text-for-you";

            var expectedResponse = new StandardResponse();
            _httpTest.RespondWithJson(expectedResponse);

            // when
            await _chatClient.PostMessage(slackKey, channel, text, null);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)));
            _httpTest
                .ShouldHaveCalled(ClientConstants.HANDSHAKE_PATH.AppendPathSegment(FlurlChatClient.SEND_MESSAGE_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("channel", channel)
                .WithQueryParamValue("text", text)
                .WithQueryParamValue("as_user", "true")
                .Times(1);
        }
    }
}