namespace SlackConnector.Models
{
    public interface ISlackReaction
    {
        string RawData { get; set; }
        SlackUser User { get; set; }
        double Timestamp { get; set; }
        string Reaction { get; set; }
        SlackReactionType ReactionType { get; }
    }
}
