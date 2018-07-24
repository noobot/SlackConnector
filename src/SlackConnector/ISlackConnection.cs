using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SlackConnector.EventHandlers;
using SlackConnector.Models;

namespace SlackConnector
{
    public interface ISlackConnection
    {
        /// <summary>
        /// All of the ChatHubs that are currently open.
        /// </summary>
        IReadOnlyDictionary<string, SlackChatHub> ConnectedHubs { get; }

        /// <summary>
        /// UserId => User object.
        /// </summary>
        IReadOnlyDictionary<string, SlackUser> UserCache { get; }

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

        /// <summary>
        /// Close websocket connection to Slack
        /// </summary>
        Task Close();

        /// <summary>
        /// Send message to Slack channel.
        /// </summary>
        Task Say(BotMessage message);

        /// <summary>
        /// Uploads a file from to a Slack channel
        /// </summary>
        Task Upload(SlackChatHub chatHub, string filePath);

        /// <summary>
        /// Uploads a stream to a Slack channel as a file
        /// </summary>
        Task Upload(SlackChatHub chatHub, Stream stream, string fileName);

        /// <summary>
        /// Get all channels and groups info.
        /// </summary>
        /// <returns>Channels and groups.</returns>
        Task<IEnumerable<SlackChatHub>> GetChannels();

        /// <summary>
        /// Get users with online status.
        /// </summary>
        /// <returns>Users.</returns>
        Task<IEnumerable<SlackUser>> GetUsers();

        /// <summary>
        /// Opens a DM channel to a user. Required to PM someone.
        /// </summary>
        Task<SlackChatHub> JoinDirectMessageChannel(string user);

        /// <summary>
        /// Joins a channel.
        /// </summary>
        Task<SlackChatHub> JoinChannel(string channelName);

        /// <summary>
        /// Create a channel.
        /// </summary>
        Task<SlackChatHub> CreateChannel(string channelName);

        /// <summary>
        /// Archives a channel.
        /// </summary>
        Task ArchiveChannel(string channelName);

        /// <summary>
        /// Sets a channel purpose.
        /// </summary>
        Task<SlackPurpose> SetChannelPurpose(string channelName, string purpose);

        /// <summary>
        /// Sets a channel topic.
        /// </summary>
        Task<SlackTopic> SetChannelTopic(string channelName, string topic);

        /// <summary>
        /// Indicate to the users on the channel that the bot is 'typing' on the keyboard.
        /// </summary>
        Task IndicateTyping(SlackChatHub chatHub);

        /// <summary>
        /// Sends a Ping message to the server to detect if the client is disconnected
        /// </summary>
        /// <returns></returns>
        Task Ping();

        /// <summary>
        /// Downloads a file from slack. Best used in conjunction with the file class found in SlackMessage
        /// </summary>
        Task<Stream> DownloadFile(Uri downloadUri);

        /// <summary>
        /// Raised when the websocket disconnects from the mothership.
        /// </summary>
        event DisconnectEventHandler OnDisconnect;

        /// <summary>
        /// Raised when attempting to reconnect to Slack after a disconnect is detected
        /// </summary>
        event ReconnectEventHandler OnReconnecting;

        /// <summary>
        /// Raised when a connection has been restored.
        /// </summary>
        event ReconnectEventHandler OnReconnect;

        /// <summary>
        /// Raised when real-time messages are received.
        /// </summary>
        event MessageReceivedEventHandler OnMessageReceived;

        /// <summary>
        /// Raised when reaction messages are received.
        /// </summary>
        event ReactionReceivedEventHandler OnReaction;

        /// <summary>
        /// Raised when bot joins a channel or group
        /// </summary>
        event ChatHubJoinedEventHandler OnChatHubJoined;

        /// <summary>
        /// Raised when a new user joins the team
        /// </summary>
        event UserJoinedEventHandler OnUserJoined;

        /// <summary>
        /// Raised when SlackApi sends a pong to our ping
        /// </summary>
        event PongEventHandler OnPong;

        /// <summary>
        /// Raised when a new channel is created
        /// </summary>
        event ChannelCreatedHandler OnChannelCreated;
    }
}