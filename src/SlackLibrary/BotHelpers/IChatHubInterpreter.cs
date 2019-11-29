using SlackLibrary.Models;

namespace SlackLibrary.BotHelpers
{
    public interface IChatHubInterpreter
    {
        SlackChatHub FromId(string hubId);
    }
}