namespace SlackConnector.Models
{
    public class SlackFileCommentReaction : ISlackReaction
    {
        public string RawData { get; set; }
        public SlackUser User { get; set; }
        public double Timestamp { get; set; }
        public string Reaction { get; set; }
        public string File { get; set; }
        public string FileComment { get; set; }
        public SlackReactionType ReactionType { get { return SlackReactionType.file_comment; } }
    }
}