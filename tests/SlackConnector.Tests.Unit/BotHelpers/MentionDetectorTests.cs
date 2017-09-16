using SlackConnector.BotHelpers;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.BotHelpers
{
    public class MentionDetectorTests
    {
        [Theory]
        [InlineData("sir-name")]
        [InlineData("<@my-id>")]
        [InlineData("SIR-name")]
        [InlineData("Hello sir-name, how you doing?")]
        [InlineData("Hello SIR-name, how you doing?")]
        [InlineData("I love <@my-id>")]
        public void should_detect_when_mentioned(string messageText)
        {
            // given
            const string userId = "my-id";
            const string userName = "sir-name";

            // when
            var detector = new MentionDetector();
            bool detected = detector.WasBotMentioned(userName, userId, messageText);

            // then
            detected.ShouldBeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("something unrelated")]
        public void should_not_detect_when_not_mentioned(string messageText)
        {
            // given
            const string userId = "my-id";
            const string userName = "sir-name";

            // when
            var detector = new MentionDetector();
            bool detected = detector.WasBotMentioned(userName, userId, messageText);

            // then
            detected.ShouldBeFalse();
        }
    }
}