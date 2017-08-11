using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.File;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    public class FlurlFileClientTests : IDisposable
    {
        private readonly HttpTest _httpTest;
        private readonly Mock<IResponseVerifier> _responseVerifierMock;
        private readonly FlurlFileClient _fileClient;
        
        public FlurlFileClientTests()
        {
            _httpTest = new HttpTest();
            _responseVerifierMock = new Mock<IResponseVerifier>();
            _fileClient = new FlurlFileClient(_responseVerifierMock.Object);
        }
        
        public void Dispose()
        {
            _httpTest.Dispose();
        }

        [Fact]
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
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlFileClient.FILE_UPLOAD_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("channels", channel)
                .WithVerb(HttpMethod.Post)
                .WithContentType("multipart/form-data")
                .Times(1);
        }
    }
}