namespace SlackConnector.Models
{
    public class SlackUnknownReaction : ISlackReaction
    {
        public string RawData { get; set; }
        public SlackUser User { get; set; }
        public double Timestamp { get; set; }
        public string Reaction { get; set; }
    }
}
