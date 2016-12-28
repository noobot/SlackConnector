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
        public void should_upload_to_channel()
        {
            // given
            var config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            var connection = slackConnector.Connect(config.Slack.ApiToken).Result;
            var fileupload = new BotFileUpload
            {
                File = Directory.GetCurrentDirectory() + "\\UploadTest.txt",
                ChatHub = connection.ConnectedChannels().First(x => x.Name.Equals(config.Slack.TestChannel, StringComparison.InvariantCultureIgnoreCase))
            };

            // when
            connection.Upload(fileupload).Wait();

            // then
        }
    }
}
