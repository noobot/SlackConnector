using System.Collections.Generic;
using System.Text;

namespace SlackConnector.BotHelpers
{
    public class BotNameRegexComposer
    {
        public string ComposeFor(string botName, string botUserId, IEnumerable<string> aliases)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"(<@" + botUserId + @">|");
            builder.Append(@"\b" + botName + @"\b");

            foreach (string alias in aliases)
            {
                builder.Append(@"|\b" + alias + @"\b");
            }
            builder.Append(@")");
            return builder.ToString();
        }
    }
}