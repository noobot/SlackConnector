namespace SlackConnector.Models
{
    public class SlackMessageReaction : ISlackReaction
    {
        public SlackChatHub ChatHub { get; set; }
        public string RawData { get; set; }
        public SlackUser User { get; set; }
        public double Timestamp { get; set; }
        public string Reaction { get; set; }
        public double ReactingToTimestamp { get; set; }
    }
}