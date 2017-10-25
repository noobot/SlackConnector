namespace SlackConnector.Models
{
    public interface ISlackReaction
    {
        string RawData { get; }
        SlackUser User { get; }
        double Timestamp { get; }
        string Reaction { get; }
        SlackUser ReactingToUser { get; }
    }
}
