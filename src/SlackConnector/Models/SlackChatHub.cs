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
        public string[] Members { get; set; }
    }
}