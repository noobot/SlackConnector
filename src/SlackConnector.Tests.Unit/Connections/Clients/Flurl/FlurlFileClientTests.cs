using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using NUnit.Framework;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Clients.File;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.TestExtensions;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    [TestFixture]
    public class FlurlFileClientTests
    {
        private HttpTest _httpTest;
        private Mock<IResponseVerifier> _responseVerifierMock;
        private FlurlFileClient _fileClient;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
            _responseVerifierMock = new Mock<IResponseVerifier>();
            _fileClient = new FlurlFileClient(_responseVerifierMock.Object);
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
            const string filePath = @"C:\test-file-name.exe";

            var expectedResponse = new StandardResponse();
            _httpTest.RespondWithJson(expectedResponse);

            // when
            await _fileClient.PostFile(slackKey, channel, filePath);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)));
            _httpTest
                .ShouldHaveCalled(ClientConstants.HANDSHAKE_PATH.AppendPathSegment(FlurlFileClient.FILE_UPLOAD_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("channels", channel)
                //.WithQueryParamValue("filename", "test-file-name.exe")
                .WithVerb(HttpMethod.Post)
                .WithContentType("multipart/form-data")
                .Times(1);
        }
    }
}