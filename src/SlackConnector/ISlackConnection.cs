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

        [Obsolete("This may be removed in the future, currently this does nothing")]
        string[] Aliases { get; set; }

        IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; }
        IReadOnlyDictionary<string, string> UserNameCache { get; }

        bool IsConnected { get; }
        DateTime? ConnectedSince { get; }
        string SlackKey { get; }

        ContactDetails Team { get; }
        ContactDetails Self { get; }

        #endregion
        
        void Disconnect();
        Task Say(BotMessage message);
        Task<SlackChatHub> JoinDirectMessageChannel(string user);

        event ConnectionStatusChangedEventHandler OnConnectionStatusChanged;
        event MessageReceivedEventHandler OnMessageReceived;
    }
}