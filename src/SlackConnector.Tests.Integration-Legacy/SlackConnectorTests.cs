using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Models;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class SlackConnectorTests : IntegrationTest
    {
        [Test]
        public async Task should_connect_and_stuff()
        {
            // given

            // when
            SlackConnection.OnDisconnect += SlackConnector_OnDisconnect;
            SlackConnection.OnMessageReceived += SlackConnectorOnMessageReceived;

            // then
            Assert.That(SlackConnection.IsConnected, Is.True);
            //Thread.Sleep(TimeSpan.FromMinutes(1));

            // when
            await SlackConnection.Close();

            Assert.That(SlackConnection.IsConnected, Is.False);
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