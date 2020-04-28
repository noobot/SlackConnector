using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExpectedObjects;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using Shouldly;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.File;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;
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


        [Fact]
        public async Task should_download_file_with_given_slack_key()
        {
            // given
            const string slackKey = "something-that-looks-like-a-slack-key";
            var expectedResponse = new StandardResponse();
            _httpTest.RespondWithJson(expectedResponse);


            // TODO: Mock a SlackFile with a Name and download link
            var mockFile = new Mock<SlackFile>();
            var uri = new Uri("https://slack.com/bar/download.png");
            mockFile.SetupGet<Uri>(foo => foo.UrlPrivateDownload).Returns(uri);
            mockFile.SetupGet<string>(foo => foo.Name).Returns("download.png");

            var file = mockFile.Object;
            //


            // .png first eight bytes
            var byteArr = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
            var content = new ByteArrayContent(byteArr);
            _httpTest.RespondWith(content);

            await _fileClient.DownloadFile(slackKey, file , file.Name);

            _httpTest.ShouldHaveCalled(uri.ToString())
                .WithOAuthBearerToken(slackKey)
                .WithVerb(HttpMethod.Get)
                .Times(1);

            // Test if download was executed correctly
            using (var fs = new FileStream(file.Name, FileMode.Open))
            {
                var readBytes = new byte[8];
                int count = await fs.ReadAsync(readBytes);

                Assert.Equal(8, count);
                Assert.Equal(byteArr, readBytes);
            }
                
        }
    }
}