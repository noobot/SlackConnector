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

        string[] Aliases { get; }
        SlackChatHub[] ConnectedDMs { get; }
        SlackChatHub[] ConnectedChannels { get; }
        SlackChatHub[] ConnectedGroups { get; }
        IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; }
        bool IsConnected { get; }
        DateTime? ConnectedSince { get; }
        Dictionary<string, object> ResponseContext { get; }
        string SlackKey { get; }
        string TeamId { get; }
        string TeamName { get; }
        string UserId { get; }
        string UserName { get; }

        #endregion


        Task Connect(string slackKey);
        void Disconnect();
        Task Say(BotMessage message);

        event ConnectionStatusChangedEventHandler ConnectionStatusChanged;
        event MessageReceivedEventHandler MessageReceived;
    }
}