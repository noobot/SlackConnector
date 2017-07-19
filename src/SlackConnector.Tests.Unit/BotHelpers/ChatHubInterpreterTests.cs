using NUnit.Framework;
using Should;
using SlackConnector.BotHelpers;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.TestExtensions;

namespace SlackConnector.Tests.Unit.BotHelpers
{
    public class ChatHubInterpreterTests
    {
        [TestCase("C-hannelId", SlackChatHubType.Channel)]
        [TestCase("D-Something", SlackChatHubType.DM)]
        [TestCase("G-roup", SlackChatHubType.Group)]
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

        [Test]
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

        [TestCase(null)]
        [TestCase("")]
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