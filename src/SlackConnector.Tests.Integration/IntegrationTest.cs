using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    public abstract class IntegrationTest
    {
        protected ISlackConnection SlackConnection;
        protected Config Config;

        [SetUp]
        public virtual async Task SetUp()
        {
            Config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            SlackConnection = await slackConnector.Connect(Config.Slack.ApiToken);
        }

        [TearDown]
        public virtual async Task TearDown()
        {
            await SlackConnection.Close();
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}