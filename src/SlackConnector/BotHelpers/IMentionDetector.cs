namespace SlackConnector.BotHelpers
{
    internal interface IMentionDetector
    {
        bool WasBotMentioned(string username, string userId, string messageText);
    }
}