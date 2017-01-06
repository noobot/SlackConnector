using System;
using System.Linq;
using System.IO;
using NUnit.Framework;
using SlackConnector.Models;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    class FileUploadTests
    {
        [Test]
        public void should_upload_to_channel_from_file_system()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;
            var fileupload = new BotFileUpload
            {
                File = Directory.GetCurrentDirectory() + "\\" + config.Slack.TestFile,
                ChatHub = connection.ConnectedChannels().First(x => x.Name.Equals(config.Slack.TestChannel, StringComparison.InvariantCultureIgnoreCase))
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
            var filePath = Directory.GetCurrentDirectory() + "\\" + config.Slack.TestFile;
            using (var fileStream = File.Open(filePath, FileMode.Open))
            {
                var fileupload = new BotStreamUpload()
                {
                    FileName = config.Slack.TestFile,
                    Stream = fileStream,
                    ChatHub =
                        connection.ConnectedChannels()
                            .First(
                                x =>
                                    x.Name.Equals(config.Slack.TestChannel, StringComparison.InvariantCultureIgnoreCase))
                };

                // when
                connection.Upload(fileupload).Wait();
            }
            // then
        }

    }
}
