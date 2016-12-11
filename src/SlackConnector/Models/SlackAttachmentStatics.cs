using System.Collections.Generic;

namespace SlackConnector.Models
{
    public partial class SlackAttachment
    {
        public const string MARKDOWN_IN_PRETEXT = "pretext";
        public const string MARKDOWN_IN_TEXT = "text";
        public const string MARKDOWN_IN_FIELDS = "fields";

        public static List<string> GetAllMarkdownInTypes()
        {
            return new List<string>
            {
                MARKDOWN_IN_FIELDS,
                MARKDOWN_IN_PRETEXT,
                MARKDOWN_IN_TEXT
            };
        }
    }
}