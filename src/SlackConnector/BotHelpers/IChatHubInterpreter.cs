using SlackConnector.Models;

namespace SlackConnector.BotHelpers
{
    public interface IChatHubInterpreter
    {
        SlackChatHub FromId(string hubId);
    }
}