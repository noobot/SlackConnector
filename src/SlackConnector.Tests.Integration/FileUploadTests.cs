using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Tests.Integration.Resources;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class FileUploadTests : IntegrationTest
    {
        private string _filePath;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _filePath = Path.GetTempFileName();
            File.WriteAllText(_filePath, EmbeddedResourceFileReader.ReadEmbeddedFileAsText("UploadTest.txt"));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            File.Delete(_filePath);
        }

        [Test]
        public async Task should_upload_to_channel_from_file_system()
        {
            // given
            var chatHub = SlackConnection.ConnectedChannel(Config.Slack.TestChannel);

            // when
            await SlackConnection.Upload(chatHub, _filePath);

            // then
        }

        [Test]
        public async Task should_upload_to_channel_from_stream()
        {
            // given
            var chatHub = SlackConnection.ConnectedChannel(Config.Slack.TestChannel);
            const string fileName = "slackconnector-test-stream-upload.txt";

            // when
            using (var fileStream = EmbeddedResourceFileReader.ReadEmbeddedFile("UploadTest.txt"))
            {
                await SlackConnection.Upload(chatHub, fileStream, fileName);
            }

            // then
        }
    }
}
