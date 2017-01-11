using System.IO;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;
using SlackConnector.Tests.Integration.Resources;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class FileUploadTests
    {
        private string _fileName;

        [SetUp]
        public void SetUp()
        {
            _fileName = Path.GetTempFileName();
            File.WriteAllText(_fileName, EmbeddedResourceFileReader.ReadEmbeddedFileAsText("UploadTest.txt"));
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_fileName);
        }

        [Test]
        public void should_upload_to_channel_from_file_system()
        {
            // given
            var config = new ConfigReader().GetConfig();
            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;

            var fileupload = new BotFileUpload
            {
                File = "slackconnector-test-file-upload.txt",
                ChatHub = connection.ConnectedChannel(config.Slack.TestChannel)
            };

            // when
            connection.Upload(fileupload).Wait();

            // then
        }

        [Test]
        public void should_upload_to_channel_from_stream()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;

            using (var fileStream = EmbeddedResourceFileReader.ReadEmbeddedFile("UploadTest.txt"))
            {
                var fileupload = new BotStreamUpload()
                {
                    FileName = "slackconnector-test-stream-upload.txt",
                    Stream = fileStream,
                    ChatHub = connection.ConnectedChannel(config.Slack.TestChannel)
                };

                // when
                connection.Upload(fileupload).Wait();
            }

            // then
        }

    }
}
