using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Core;
using SlackConnector.EventHandlers;
using SlackConnector.Models;

namespace SlackConnector.Tests.Unit.Stubs
{
    public class SlackConnectionStub : ISlackConnection
    {
        public string[] Aliases { get; set; }
        public IEnumerable<SlackChatHub> ConnectedDMs { get; }
        public IEnumerable<SlackChatHub> ConnectedChannels { get; }
        public IEnumerable<SlackChatHub> ConnectedGroups { get; }
        public IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; }
        public IReadOnlyDictionary<string, SlackUser> UserNameCache { get; }
        public IReadOnlyDictionary<string, SlackUser> UserCache { get; }
        public bool IsConnected { get; }
        public DateTime? ConnectedSince { get; }
        public string SlackKey { get; }
        public ContactDetails Team { get; }
        public ContactDetails Self { get; }
        public Task Connect(string slackKey)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public Task Say(BotMessage message)
        {
            throw new NotImplementedException();
        }
        
        public Task<IEnumerable<SlackChatHub>> GetChannels()
        {
          throw new NotImplementedException();
        }

        public Task<IEnumerable<SlackUser>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<SlackChatHub> JoinDirectMessageChannel(string user)
        {
            throw new NotImplementedException();
        }

        public Task IndicateTyping(SlackChatHub chatHub)
        {
            throw new NotImplementedException();
        }

        public Task Ping()
        {
            throw new NotImplementedException();
        }

        public event DisconnectEventHandler OnDisconnect;
        public void RaiseOnDisconnect()
        {
            OnDisconnect?.Invoke();
        }

        public event MessageReceivedEventHandler OnMessageReceived;
        public void RaiseOnMessageReceived()
        {
            OnMessageReceived?.Invoke(null);
        }

        public event ChatHubJoinedEventHandler OnChatHubJoined;
        public void RaiseOnChatHubJoined()
        {
            OnChatHubJoined?.Invoke(null);
        }

        public event UserJoinedEventHandler OnUserJoined;
        public void RaiseOnUserJoined()
        {
            OnUserJoined?.Invoke(null);
        }

        public Task Upload(SlackChatHub chatHub, string filePath)
        {
            throw new NotImplementedException();
        }

        public Task Upload(SlackChatHub chatHub, Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
