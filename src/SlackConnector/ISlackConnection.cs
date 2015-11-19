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

        /// <summary>
        /// All of the ChatHubs that are currently open.
        /// </summary>
        IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; }

        /// <summary>
        /// UserId => UserName cache.
        /// </summary>
        IReadOnlyDictionary<string, string> UserNameCache { get; }

        /// <summary>
        /// Is the RealTimeConnection currently open?
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// When did we establish the connection?
        /// </summary>
        DateTime? ConnectedSince { get; }

        /// <summary>
        /// Slack Authentication Key.
        /// </summary>
        string SlackKey { get; }

        /// <summary>
        /// Connected Team Details.
        /// </summary>
        ContactDetails Team { get; }

        /// <summary>
        /// Authenticated Self Details.
        /// </summary>
        ContactDetails Self { get; }

        #endregion
        
        /// <summary>
        /// Disconnect from Slack.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send message to Slack channel.
        /// </summary>
        Task Say(BotMessage message);

        /// <summary>
        /// Opens a DM channel to a user. Required to PM someone.
        /// </summary>
        Task<SlackChatHub> JoinDirectMessageChannel(string user);

        /// <summary>
        /// Indicate to the users on the channel that the bot is 'typing' on the keyboard.
        /// </summary>
        Task IndicateTyping(SlackChatHub chatHub);

        /// <summary>
        /// Raised when the websocket disconnects from the mothership.
        /// </summary>
        event DisconnectEventHandler OnDisconnect;

        /// <summary>
        /// Raised when real-time messages are received.
        /// </summary>
        event MessageReceivedEventHandler OnMessageReceived;
    }
}