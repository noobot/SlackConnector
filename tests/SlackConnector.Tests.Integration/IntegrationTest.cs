using System;
using System.Threading.Tasks;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    public abstract class IntegrationTest : IDisposable
    {
        protected ISlackConnection SlackConnection;
        protected Config Config;

        protected IntegrationTest()
        {
            Config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            SlackConnection = Task.Run(() => slackConnector.Connect(Config.Slack.ApiToken))
                    .GetAwaiter()
                    .GetResult();
        }
        
        public virtual void Dispose()
        {
            Task.Run(() => SlackConnection.Close())
                .GetAwaiter()
                .GetResult();
        }
    }
}