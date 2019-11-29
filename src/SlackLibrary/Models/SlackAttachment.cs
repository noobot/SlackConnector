using System.Collections.Generic;
using Newtonsoft.Json;

namespace SlackLibrary.Models
{
    public partial class SlackAttachment
    {
		[JsonProperty(PropertyName = "fallback", NullValueHandling = NullValueHandling.Ignore)]
        public string Fallback { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore)]
        public string ColorHex { get; set; }

        [JsonProperty(PropertyName = "pretext", NullValueHandling = NullValueHandling.Ignore)]
        public string PreText { get; set; }

        [JsonProperty(PropertyName = "author_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorName { get; set; }

        [JsonProperty(PropertyName = "author_link", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorLink { get; set; }

        [JsonProperty(PropertyName = "author_icon", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorIcon { get; set; }

        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "title_link", NullValueHandling = NullValueHandling.Ignore)]
        public string TitleLink { get; set; }

        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "fields", NullValueHandling = NullValueHandling.Ignore)]
        public IList<SlackAttachmentField> Fields { get; set; }

        [JsonProperty(PropertyName = "callback_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CallbackId { get; set; }

        [JsonProperty(PropertyName = "actions", NullValueHandling = NullValueHandling.Ignore)]
        public IList<SlackAttachmentAction> Actions { get; set; }

        [JsonProperty(PropertyName = "image_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "thumb_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ThumbUrl { get; set; }

        [JsonProperty(PropertyName = "mrkdwn_in", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> MarkdownIn { get; set; }

		[JsonProperty(PropertyName = "footer", NullValueHandling = NullValueHandling.Ignore)]
		public string Footer { get; set; }

		[JsonProperty(PropertyName = "footer_icon", NullValueHandling = NullValueHandling.Ignore)]
		public string FooterIcon { get; set; }

		public SlackAttachment()
        {
            Fields = new List<SlackAttachmentField>();
			Actions = new List<SlackAttachmentAction>();
		}
    }
}