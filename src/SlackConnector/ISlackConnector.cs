using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.EventHandlers;
using SlackConnector.Models;

namespace SlackConnector
{
    public interface ISlackConnector
    {
        #region Properties

        string[] Aliases { get; set; }
        SlackChatHub[] ConnectedDMs { get; }
        SlackChatHub[] ConnectedChannels { get; }
        SlackChatHub[] ConnectedGroups { get; }
        IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; }
        IReadOnlyDictionary<string, string> UserNameCache { get; }
        bool IsConnected { get; }
        DateTime? ConnectedSince { get; }
        string SlackKey { get; }
        string TeamId { get; }
        string TeamName { get; }
        string UserId { get; }
        string UserName { get; }

        #endregion


        Task Connect(string slackKey);
        void Disconnect();
        Task Say(BotMessage message);

        event ConnectionStatusChangedEventHandler OnConnectionStatusChanged;
        event MessageReceivedEventHandler OnMessageReceived;
    }
}