namespace SlackConnector.Models
{
    public class SlackFileReaction : ISlackReaction
    {
        public string RawData { get; set; }
        public SlackUser User { get; set; }
        public double Timestamp { get; set; }
        public string Reaction { get; set; }
        public string File { get; set; }
    }
}