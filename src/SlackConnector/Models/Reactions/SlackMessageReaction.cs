namespace SlackConnector.Models.Reactions
{
    public class SlackMessageReaction : ISlackReaction
    {
        public SlackChatHub ChatHub { get; internal set; }
        public string RawData { get; internal set; }
        public SlackUser User { get; internal set; }
        public double Timestamp { get; internal set; }
        public string Reaction { get; internal set; }
        public SlackUser ReactingToUser { get; internal set; }
    }
}