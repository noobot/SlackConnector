using SlackConnector.BotHelpers;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.TestExtensions;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.BotHelpers
{
    public class ChatHubInterpreterTests
    {
        [Theory]
        [InlineData("C-hannelId", SlackChatHubType.Channel)]
        [InlineData("D-Something", SlackChatHubType.DM)]
        [InlineData("G-roup", SlackChatHubType.Group)]
        public void given_hub_id_then_should_return_expected_slack_hub(string hubId, SlackChatHubType hubType)
        {
            // given

            // when
            var interpreter = new ChatHubInterpreter();
            SlackChatHub chatHub = interpreter.FromId(hubId);

            // then
            var expected = new SlackChatHub
            {
                Id = hubId,
                Name = hubId,
                Type = hubType
            };
            chatHub.ShouldLookLike(expected);
        }

        [Fact]
        public void shouldnt_return_slack_hub_if_type_cant_be_detected()
        {
            // given
            const string hubId = "SOMETHING THAT ISN'T CORRECT";

            // when
            var interpreter = new ChatHubInterpreter();
            SlackChatHub chatHub = interpreter.FromId(hubId);

            // then
            chatHub.ShouldBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void shouldnt_return_slack_hub_if_hub_id_is_null_or_empty(string hubId)
        {
            // given

            // when
            var interpreter = new ChatHubInterpreter();
            SlackChatHub chatHub = interpreter.FromId(hubId);

            // then
            chatHub.ShouldBeNull();
        }
    }
}