namespace SlackConnector
{
    public interface ISlackConnectorFactory
    {
        ISlackConnector Connect(string slackKey);
    }
}