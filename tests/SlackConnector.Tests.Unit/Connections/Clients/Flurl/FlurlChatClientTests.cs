using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using Newtonsoft.Json;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    public class FlurlChatClientTests : IDisposable
    {
        private readonly HttpTest _httpTest;
        private readonly Mock<IResponseVerifier> _responseVerifierMock;
        private readonly FlurlChatClient _chatClient;
        
        public FlurlChatClientTests()
        {
            _httpTest = new HttpTest();
            _responseVerifierMock = new Mock<IResponseVerifier>();
            _chatClient = new FlurlChatClient(_responseVerifierMock.Object);
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
            const string text = "some-text-for-you";

            var expectedResponse = new StandardResponse();
            _httpTest.RespondWithJson(expectedResponse);

            // when
            await _chatClient.PostMessage(slackKey, channel, text, null);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)));
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChatClient.SEND_MESSAGE_PATH))
                .WithOAuthBearerToken(slackKey)
                .WithRequestJson(new
                {
                    channel = channel,
                    text = text,
                    as_user = true,
                    link_names = true,
                    attachments = (object)null
                })
                .Times(1);
        }

        [Fact]
        public async Task should_add_attachments_if_given()
        {
            const string slackKey = "something-that-looks-like-a-slack-key";
            // given
            _httpTest.RespondWithJson(new StandardResponse());
            var attachments = new List<SlackAttachment>
            {
                new SlackAttachment { Text = "dummy text" },
                new SlackAttachment { AuthorName = "dummy author" },
            };

            // when
            await _chatClient.PostMessage(slackKey, It.IsAny<string>(), It.IsAny<string>(), attachments);

            // then
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChatClient.SEND_MESSAGE_PATH))
                .WithOAuthBearerToken(slackKey)
                .WithRequestJson(new
                {
                    channel = (string)null,
                    text = (string)null,
                    as_user = true,
                    link_names = true,
                    attachments = attachments
                })
                .Times(1);
        }
    }
}