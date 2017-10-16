using System;
using System.Threading.Tasks;
using ExpectedObjects;
using Flurl;
using Flurl.Http.Testing;
using Moq;
using Shouldly;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    public class FlurlChannelClientTests : IDisposable
    {
        private readonly HttpTest _httpTest;
        private readonly Mock<IResponseVerifier> _responseVerifierMock;
        private readonly FlurlChannelClient _channelClient;

        public FlurlChannelClientTests()
        {
            _httpTest = new HttpTest();
            _responseVerifierMock = new Mock<IResponseVerifier>();
            _channelClient = new FlurlChannelClient(_responseVerifierMock.Object);
        }

        public void Dispose()
        {
            _httpTest.Dispose();
        }

        [Fact]
        public async Task should_join_direct_message_channel()
        {
            // given
            const string slackKey = "I-is-another-key";
            const string userId = "blahdy-blah";

            var expectedResponse = new JoinChannelResponse
            {
                Channel = new Channel
                {
                    Id = "some-channel",
                    IsChannel = true
                }
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.JoinDirectMessageChannel(slackKey, userId);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.JOIN_DM_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("user", userId)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Channel);
        }

        [Fact]
        public async Task should_create_channel()
        {
            // given
            const string slackKey = "I-is-another-key";
            const string channelName = "my-channel";

            var expectedResponse = new ChannelResponse()
            {
                Channel = new Channel
                {
                    Id = "some-channel",
                    IsChannel = true
                }
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.CreateChannel(slackKey, channelName);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.CHANNEL_CREATE_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("name", channelName)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Channel);
        }

        [Fact]
        public async Task should_join_channel()
        {
            // given
            const string slackKey = "I-is-another-key";
            const string channelName = "my-channel";

            var expectedResponse = new ChannelResponse()
            {
                Channel = new Channel
                {
                    Id = "some-channel",
                    IsChannel = true
                }
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.JoinChannel(slackKey, channelName);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.CHANNEL_JOIN_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("name", channelName)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Channel);
        }

        [Fact]
        public async Task should_archive_channel()
        {
            // given
            const string slackKey = "I-is-another-key";
            const string channelName = "my-channel";

            var expectedResponse = new StandardResponse()
            {
                Ok = true
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            await _channelClient.ArchiveChannel(slackKey, channelName);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.CHANNEL_ARCHIVE_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("channel", channelName)
                .Times(1);
        }

        [Fact]
        public async Task should_set_channel_purpose()
        {
            // given
            const string slackKey = "I-is-another-key";
            const string channelName = "my-channel";
            const string channelPurpose = "my purpose";

            var expectedResponse = new PurposeResponse()
            {
                Purpose = channelPurpose
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.SetPurpose(slackKey, channelName, channelPurpose);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.CHANNEL_SET_PURPOSE_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("channel", channelName)
                .WithQueryParamValue("purpose", channelPurpose)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Purpose);
        }

        [Fact]
        public async Task should_set_channel_topic()
        {
            // given
            const string slackKey = "I-is-another-key";
            const string channelName = "my-channel";
            const string channelTopic = "my topic";

            var expectedResponse = new TopicResponse()
            {
                Topic = channelTopic
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.SetTopic(slackKey, channelName, channelTopic);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.CHANNEL_SET_TOPIC_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("channel", channelName)
                .WithQueryParamValue("topic", channelTopic)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Topic);
        }

        [Fact]
        public async Task should_call_expected_url_and_return_expected_channels()
        {
            // given
            const string slackKey = "I-is-da-key-yeah";

            var expectedResponse = new ChannelsResponse
            {
                Channels = new[]
                {
                    new Channel { IsChannel = true, Name = "name1" },
                    new Channel { IsArchived = true, Name = "name2" },
                }
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.GetChannels(slackKey);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.CHANNELS_LIST_PATH))
                .WithQueryParamValue("token", slackKey)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Channels);
        }

        [Fact]
        public async Task should_call_expected_url_and_return_expected_groups()
        {
            // given
            const string slackKey = "I-is-another-key";

            var expectedResponse = new GroupsResponse
            {
                Groups = new[]
                 {
                    new Group { IsGroup = true, Name = "name1" },
                    new Group { IsOpen = true, Name = "name2" }
                }
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.GetGroups(slackKey);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.GROUPS_LIST_PATH))
                .WithQueryParamValue("token", slackKey)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Groups);
        }

        [Fact]
        public async Task should_call_expected_url_and_return_expected_users()
        {
            // given
            const string slackKey = "I-is-another-key";

            var expectedResponse = new UsersResponse
            {
                Members = new[]
                {
                    new User { Id = "some-id-thing", IsBot = true },
                    new User { Name = "some-id-thing", Deleted = true },
                }
            };

            _httpTest.RespondWithJson(expectedResponse);

            // when
            var result = await _channelClient.GetUsers(slackKey);

            // then
            _responseVerifierMock.Verify(x => x.VerifyResponse(Looks.Like(expectedResponse)), Times.Once);
            _httpTest
                .ShouldHaveCalled(ClientConstants.SlackApiHost.AppendPathSegment(FlurlChannelClient.USERS_LIST_PATH))
                .WithQueryParamValue("token", slackKey)
                .WithQueryParamValue("presence", "1")
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse.Members);
        }
    }
}