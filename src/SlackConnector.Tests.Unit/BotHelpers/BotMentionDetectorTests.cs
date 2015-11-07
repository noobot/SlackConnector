using NUnit.Framework;
using Should;
using SlackConnector.BotHelpers;

namespace SlackConnector.Tests.Unit.BotHelpers
{
    [TestFixture]
    public class BotMentionDetectorTests
    {
        [TestCase("sir-name")]
        [TestCase("<@my-id>")]
        [TestCase("SIR-name")]
        [TestCase("Hello sir-name, how you doing?")]
        [TestCase("Hello SIR-name, how you doing?")]
        [TestCase("I love <@my-id>")]
        public void should_detect_when_mentioned(string messageText)
        {
            // given
            const string userId = "my-id";
            const string userName = "sir-name";

            // when
            var detector = new BotMentionDetector();
            bool detected = detector.WasBotMentioned(userName, userId, messageText);

            // then
            detected.ShouldBeTrue();
        }

        //[TestCase("sir-namey")]
        //[TestCase("with more text sir-namey")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("something unrelated")]
        public void should_not_detect_when_not_mentioned(string messageText)
        {
            // given
            const string userId = "my-id";
            const string userName = "sir-name";

            // when
            var detector = new BotMentionDetector();
            bool detected = detector.WasBotMentioned(userName, userId, messageText);

            // then
            detected.ShouldBeFalse();
        }


    }
}