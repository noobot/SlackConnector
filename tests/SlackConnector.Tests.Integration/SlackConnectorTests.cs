using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SlackConnector.Models;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Integration
{
    public class SlackConnectorTests : IntegrationTest
    {
        [Fact]
        public async Task should_connect_and_stuff()
        {
            // given

            // when
            SlackConnection.OnDisconnect += SlackConnector_OnDisconnect;
            SlackConnection.OnMessageReceived += SlackConnectorOnMessageReceived;
            SlackConnection.OnReaction += SlackConnectionOnOnReaction;

            // then
            SlackConnection.IsConnected.ShouldBeTrue();
            //Thread.Sleep(TimeSpan.FromMinutes(1));

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