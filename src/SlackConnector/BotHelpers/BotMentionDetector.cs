namespace SlackConnector.BotHelpers
{
    internal class BotMentionDetector : IBotMentionDetector
    {
        //private bool BotMentioned(string messageText)
        //{
        //    bool mentioned = false;

        //    // only build the regex if we're connected - if we're not connected we won't know our bot's name or user Id
        //    if (IsConnected)
        //    {
        //        string regex = new BotNameRegexComposer().ComposeFor(UserName, UserId, new string[0]);
        //        mentioned = (messageText != null && Regex.IsMatch(messageText, regex, RegexOptions.IgnoreCase));
        //    }

        //    return mentioned;
        //}

        //public string ComposeFor(string botName, string botUserId, IEnumerable<string> aliases)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append(@"(<@" + botUserId + @">|");
        //    builder.Append(@"\b" + botName + @"\b");

        //    foreach (string alias in (aliases ?? new string[0]))
        //    {
        //        builder.Append(@"|\b" + alias + @"\b");
        //    }

        //    builder.Append(@")");
        //    return builder.ToString();
        //}
        
        public bool WasBotMentioned(string username, string userId, string messageTexr)
        {
            throw new System.NotImplementedException();
        }
    }
}