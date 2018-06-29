using Newtonsoft.Json;

namespace SlackConnector.Models
{
    public class SlackAttachmentAction
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "style", NullValueHandling = NullValueHandling.Ignore)]
        public SlackAttachmentActionStyle? Style { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }


        public SlackAttachmentAction()
        {
            Type = "button";
        }
    }
}