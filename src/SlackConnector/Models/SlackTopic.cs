namespace SlackConnector.Models
{
    /// <summary>
    /// This represents the topic value of the requested channel.
    /// </summary>
    public class SlackTopic
    {
        public string ChannelName { get; set; }
        public string Topic { get; set; }
    }
}
