using SlackConnector.Models;

namespace SlackConnector.BotHelpers
{
    internal interface IChatHubInterpreter
    {
        SlackChatHub FromId(string hubId);
    }
}