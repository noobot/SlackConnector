namespace SlackConnector.Tests.Integration.Configuration
{
    public class Config
    {
        public SlackConfig Slack { get; set; }
        public bool IsConfigured
        {
            get
            {
                return Slack != null
                    && string.IsNullOrWhiteSpace(Slack.ApiToken) == false
                    && string.IsNullOrWhiteSpace(Slack.TestUserId) == false;
            }
        }
    }
}