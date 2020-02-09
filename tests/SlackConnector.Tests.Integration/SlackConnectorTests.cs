using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SlackConnector.Models;
using Xunit;
using Shouldly;
using SlackConnector.Tests.Integration.Configuration;

namespace SlackConnector.Tests.Integration
{
    public class SlackConnectorTests : IntegrationTest
    {
        [Fact]
        public async Task should_connect()
        {
            SlackConnection.IsConnected.ShouldBeTrue();

            await SlackConnection.Close();

            SlackConnection.IsConnected.ShouldBeFalse();
        }

        [RunnableInDebugOnly]
        public async Task connect_and_wait_5_mins()
        {
            // given

            // when
            SlackConnection.OnDisconnect += SlackConnector_OnDisconnect;
            SlackConnection.OnMessageReceived += SlackConnectorOnMessageReceived;
            SlackConnection.OnReaction += SlackConnectionOnOnReaction;

            // then
            SlackConnection.IsConnected.ShouldBeTrue();
            await Task.Delay(TimeSpan.FromMinutes(5));

            // when
            await SlackConnection.Close();

            SlackConnection.IsConnected.ShouldBeFalse();
        }

        private Task SlackConnectionOnOnReaction(ISlackReaction message)
        {
            return Task.CompletedTask;
        }

        private void SlackConnector_OnDisconnect()
        {

        }

        private Task SlackConnectorOnMessageReceived(SlackMessage message)
        {
            Debug.WriteLine(message.Text);
            Console.WriteLine(message.Text);
            return Task.CompletedTask;
        }
    }
}