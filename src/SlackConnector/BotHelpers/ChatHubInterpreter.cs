using SlackConnector.Models;

namespace SlackConnector.BotHelpers
{
    public class ChatHubInterpreter : IChatHubInterpreter
    {
        public SlackChatHub FromId(string hubId)
        {
            if (!string.IsNullOrEmpty(hubId))
            {
                SlackChatHubType? hubType = null;

                switch (hubId.ToCharArray()[0])
                {
                    case 'C':
                        hubType = SlackChatHubType.Channel;
                        break;
                    case 'D':
                        hubType = SlackChatHubType.DM;
                        break;
                    case 'G':
                        hubType = SlackChatHubType.Group;
                        break;
                }

                if (hubType != null)
                {
                    return new SlackChatHub()
                    {
                        Id = hubId,
                        Name = hubId,
                        Type = hubType.Value
                    };
                }
            }

            return null;
        }
    }
}