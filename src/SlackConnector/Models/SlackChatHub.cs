namespace SlackConnector.Models
{
    /// <summary>
    /// This represents a place in Slack where people can chat - typically a channel, group, or DM.
    /// </summary>
    public class SlackChatHub
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SlackChatHubType Type { get; set; }

        public static SlackChatHub FromId(string hubId)
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