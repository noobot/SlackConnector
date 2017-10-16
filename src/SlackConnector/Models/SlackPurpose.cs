namespace SlackConnector.Models
{
    /// <summary>
    /// This represents the purpose value of the requested channel.
    /// </summary>
    public class SlackPurpose
    {
        public string ChannelName { get; set; }
        public string Purpose { get; set; }
    }
}
