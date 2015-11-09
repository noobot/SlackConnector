using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.EventHandlers;
using SlackConnector.Models;

namespace SlackConnector
{
    public interface ISlackConnection
    {
        #region Properties

        string[] Aliases { get; set; }
        IEnumerable<SlackChatHub> ConnectedDMs { get; }
        IEnumerable<SlackChatHub> ConnectedChannels { get; }
        IEnumerable<SlackChatHub> ConnectedGroups { get; }
        IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; }
        IReadOnlyDictionary<string, string> UserNameCache { get; }
        bool IsConnected { get; }
        DateTime? ConnectedSince { get; }
        string SlackKey { get; }

        ContactDetails Team { get; }
        ContactDetails Self { get; }

        #endregion


        Task Connect(string slackKey);
        void Disconnect();
        Task Say(BotMessage message);
        Task<SlackChatHub> JoinDirectMessageChannel(string user);

        event ConnectionStatusChangedEventHandler OnConnectionStatusChanged;
        event MessageReceivedEventHandler OnMessageReceived;
    }
}