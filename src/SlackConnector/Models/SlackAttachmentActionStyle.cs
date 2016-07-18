using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SlackConnector.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SlackAttachmentActionStyle
    {
        [EnumMember(Value = "default")]
        Default,
        [EnumMember(Value = "primary")]
        Primary,
        [EnumMember(Value = "danger")]
        Danger
    }
}