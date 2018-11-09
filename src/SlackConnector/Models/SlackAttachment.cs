using System.Collections.Generic;
using Newtonsoft.Json;

namespace SlackConnector.Models
{
    public partial class SlackAttachment
    {
        [JsonProperty(PropertyName = "fallback")]
        public string Fallback { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string ColorHex { get; set; }

        [JsonProperty(PropertyName = "pretext")]
        public string PreText { get; set; }

        [JsonProperty(PropertyName = "author_name")]
        public string AuthorName { get; set; }

        [JsonProperty(PropertyName = "author_link")]
        public string AuthorLink { get; set; }

        [JsonProperty(PropertyName = "author_icon")]
        public string AuthorIcon { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "title_link")]
        public string TitleLink { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public IList<SlackAttachmentField> Fields { get; set; }

        [JsonProperty(PropertyName = "callback_id")]
        public string CallbackId { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public IList<SlackAttachmentAction> Actions { get; set; }

        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "thumb_url")]
        public string ThumbUrl { get; set; }

        [JsonProperty(PropertyName = "footer")]
        public string Footer { get; set; }

        [JsonProperty(PropertyName = "footer_icon")]
        public string FooterIcon { get; set; }

        [JsonProperty(PropertyName = "mrkdwn_in")]
        public List<string> MarkdownIn { get; set; }

        public SlackAttachment()
        {
            Fields = new List<SlackAttachmentField>();
        }
    }
}