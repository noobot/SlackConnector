using System.Threading.Tasks;
using ExpectedObjects;
using Flurl;
using Flurl.Http.Testing;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Tests.Unit.Connections.Clients.Flurl
{
    [TestFixture]
    public class FlurlChannelClientTests
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
        public async Task should_call_expected_url_and_return_expected_channels()
        {
            // given
            const string slackKey = "I-is-da-key-yeah";

            var expectedResponse = new []
            {
                new Channel { IsChannel = true, Name = "name1" },
                new Channel { IsArchived = true, Name = "name2" },
            };

            _httpTest.RespondWithJson(expectedResponse);
            var client = new FlurlChannelClient();

            // when
            var result = await client.GetChannels(slackKey);

            // then
            _httpTest
                .ShouldHaveCalled(ClientConstants.HANDSHAKE_PATH.AppendPathSegment(FlurlChannelClient.CHANNELS_LIST_PATH))
                .WithQueryParamValue("token", slackKey)
                .Times(1);

            result.ToExpectedObject().ShouldEqual(expectedResponse);
        }
    }
}