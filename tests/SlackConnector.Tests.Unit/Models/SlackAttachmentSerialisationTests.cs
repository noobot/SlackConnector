using Newtonsoft.Json;
using Shouldly;
using SlackConnector.Models;
using System.Text.RegularExpressions;
using Xunit;

namespace SlackConnector.Tests.Unit.Models
{
    public class SlackAttachmentSerialisationTests
    {
        [Fact]
        public void should_return_expected_json_when_serialised()
        {
            // given
            string expectedJson = Resources.ResourceManager.GetAttachmentsJson();
            expectedJson = RemoveLinesAndStuffFromJson(expectedJson);

            var attachment =
                new SlackAttachment
                {
                    Fallback = "Required plain-text summary of the attachment.",
                    ColorHex = "#36a64f",
                    PreText = "Optional text that appears above the attachment block",
                    AuthorName = "Bobby Tables",
                    AuthorLink = "http://flickr.com/bobby/",
                    AuthorIcon = "http://flickr.com/icons/bobby.jpg",
                    Title = "Slack API Documentation",
                    TitleLink = "https://api.slack.com/",
                    Text = "Optional text that appears within the attachment",
                    CallbackId = "mycallbackid",
                    MarkdownIn = SlackAttachment.GetAllMarkdownInTypes(),
                    Fields = new[]
                    {
                        new SlackAttachmentField
                        {
                            IsShort = true,
                            Title = "Priority",
                            Value = "High"
                        }
                    },
                    Actions = new[]
                    {
                        new SlackAttachmentAction
                        {
                            Name = "yes",
                            Value = "yep",
                            Text = "Yes",
                            Style = SlackAttachmentActionStyle.Primary
                        },
                        new SlackAttachmentAction
                        {
                            Name = "no",
                            Value = "nop",
                            Text = "No"
                        },
                        new SlackAttachmentAction
                        {
                            Url = "https://test.com/",
                            Text = "LinkButton"
                        }
                    },
                    ImageUrl = "http://my-website.com/path/to/image.jpg",
                    ThumbUrl = "http://example.com/path/to/thumb.png",
                    Footer = "Brief text to help contextualize an attachment",
                    FooterIcon = "http://flickr.com/icons/footer.jpg"
                };

            // when
            string resultJson = JsonConvert.SerializeObject(attachment);
            resultJson = RemoveLinesAndStuffFromJson(resultJson);

            // then
            resultJson.ShouldBe(expectedJson);
        }

        private static string RemoveLinesAndStuffFromJson(string json)
        {
            return Regex.Replace(json, @"(""(?:[^""\\]|\\.)*"")|\s+", "$1");
        }
    }
}