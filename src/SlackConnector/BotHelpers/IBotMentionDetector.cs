namespace SlackConnector.BotHelpers
{
    internal interface IBotMentionDetector
    {
        bool WasBotMentioned(string username, string userId, string messageText);
    }
}