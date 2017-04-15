using System;
using System.Threading;
using NUnit.Framework;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    public abstract class IntegrationTest
    {
        protected ISlackConnection SlackConnection;
        protected Config Config;

        [SetUp]
        public virtual void SetUp()
        {
            Config = new ConfigReader().GetConfig();

            var slackConnector = new SlackConnector();
            SlackConnection = slackConnector.Connect(Config.Slack.ApiToken).Result;
        }

        [TearDown]
        public virtual void TearDown()
        {
            SlackConnection.Close().Wait();
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}