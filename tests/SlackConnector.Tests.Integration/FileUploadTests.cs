using System.IO;
using System.Threading.Tasks;
using SlackConnector.Tests.Integration.Resources;
using Xunit;

namespace SlackConnector.Tests.Integration
{
    public class FileUploadTests : IntegrationTest
    {
        private readonly string _filePath;
        
        public FileUploadTests()
        {
            _filePath = Path.GetTempFileName();
            File.WriteAllText(_filePath, EmbeddedResourceFileReader.ReadEmbeddedFileAsText("UploadTest.txt"));
        }
        
        public override void Dispose()
        {
            base.Dispose();
            File.Delete(_filePath);
        }

        [Fact]
        public async Task should_upload_to_channel_from_file_system()
        {
            // given
            var chatHub = SlackConnection.ConnectedChannel(Config.Slack.TestChannel);

            // when
            await SlackConnection.Upload(chatHub, _filePath);

            // then
        }

        [Fact]
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
