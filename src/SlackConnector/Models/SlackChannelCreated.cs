namespace SlackConnector.Models
{
    public class SlackChannelCreated
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public SlackUser Creator { get; internal set; }
    }
}