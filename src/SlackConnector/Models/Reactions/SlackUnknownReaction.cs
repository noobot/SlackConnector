namespace SlackConnector.Models.Reactions
{
    public class SlackUnknownReaction : ISlackReaction
    {
        public string RawData { get; internal set; }
        public SlackUser User { get; internal set; }
        public double Timestamp { get; internal set; }
        public string Reaction { get; internal set; }
        public SlackUser ReactingToUser { get; internal set; }
    }
}
