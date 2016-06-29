using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SlackConnector.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SlackAttachmentActionStyle
    {
        @default,
        primary,
        danger
    }
}