using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using SlackLibrary.BotHelpers;
using SlackLibrary.Connections;
using SlackLibrary.Connections.Clients.Channel;
using SlackLibrary.Connections.Models;
using SlackLibrary.Connections.Monitoring;
using SlackLibrary.Connections.Sockets;
using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Connections.Sockets.Messages.Outbound;
using SlackLibrary.EventHandlers;
using SlackLibrary.Exceptions;
using SlackLibrary.Extensions;
using SlackLibrary.Models;
using SlackLibrary.Connections.Sockets.Messages.Inbound.ReactionItem;
using SlackLibrary.Models.Reactions;

namespace SlackLibrary
{
    internal partial class SlackConnection : ISlackConnection
    {
        private Task ListenTo(InboundMessage inboundMessage)
        {
            if (inboundMessage == null)
            {
                return Task.CompletedTask;
            }

            //TODO: Visitor pattern?
            switch (inboundMessage.MessageType)
            {
                case MessageType.Message: return HandleMessage((ChatMessage)inboundMessage);
                case MessageType.Group_Joined: return HandleGroupJoined((GroupJoinedMessage)inboundMessage);
                case MessageType.Channel_Joined: return HandleChannelJoined((ChannelJoinedMessage)inboundMessage);
                case MessageType.Im_Created: return HandleDmJoined((DmChannelJoinedMessage)inboundMessage);
                case MessageType.Team_Join: return HandleUserJoined((UserJoinedMessage)inboundMessage);
                case MessageType.Pong: return HandlePong((PongMessage)inboundMessage);
                case MessageType.Reaction_Added: return HandleReaction((ReactionMessage)inboundMessage);
                case MessageType.Channel_Created: return HandleChannelCreated((ChannelCreatedMessage)inboundMessage);
                case MessageType.Presence_Change: return HandlePresenceChange((PresenceChangeMessage)inboundMessage);
            }

            return Task.CompletedTask;
        }

        private Task HandleMessage(ChatMessage inboundMessage)
        {
            if (string.IsNullOrEmpty(inboundMessage.User))
                return Task.CompletedTask;

            if (!string.IsNullOrEmpty(Self.Id) && inboundMessage.User == Self.Id)
                return Task.CompletedTask;

            //TODO: Insert into connectedHubs when DM is missing

            var message = new SlackMessage
            {
                User = GetMessageUser(inboundMessage.User),
                Timestamp = inboundMessage.Timestamp,
				ThreadTimestamp = inboundMessage.ThreadTimestamp,
                Text = inboundMessage.Text,
                ChatHub = GetChatHub(inboundMessage.Channel),
                RawData = inboundMessage.RawData,
                MentionsBot = _mentionDetector.WasBotMentioned(Self.Name, Self.Id, inboundMessage.Text),
				MessageSubType = inboundMessage.MessageSubType.ToSlackMessageSubType(),
				Files = inboundMessage.Files.ToSlackFiles()
			};

            return RaiseMessageReceived(message);
        }

        private Task HandleReaction(ReactionMessage inboundMessage)
        {
            if (string.IsNullOrEmpty(inboundMessage.User))
                return Task.CompletedTask;

            if (!string.IsNullOrEmpty(Self.Id) && inboundMessage.User == Self.Id)
                return Task.CompletedTask;

            if (inboundMessage.ReactingTo is MessageReaction messageReaction)
            {
                //TODO: Interface methods? Extension methods?
                return RaiseReactionReceived(
                    new SlackMessageReaction
                    {
                        User = GetMessageUser(inboundMessage.User),
                        Timestamp = inboundMessage.Timestamp,
                        ChatHub = GetChatHub(messageReaction.Channel),
                        RawData = inboundMessage.RawData,
                        Reaction = inboundMessage.Reaction,
                        ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                    });
            }

            if (inboundMessage.ReactingTo is FileReaction fileReaction)
            {
                return RaiseReactionReceived(
                    new SlackFileReaction
                    {
                        User = GetMessageUser(inboundMessage.User),
                        Timestamp = inboundMessage.Timestamp,
                        File = fileReaction.File,
                        RawData = inboundMessage.RawData,
                        Reaction = inboundMessage.Reaction,
                        ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                    });
            }

            if (inboundMessage.ReactingTo is FileCommentReaction fileCommentReaction)
            {
                return RaiseReactionReceived(
                    new SlackFileCommentReaction
                    {
                        User = GetMessageUser(inboundMessage.User),
                        Timestamp = inboundMessage.Timestamp,
                        File = fileCommentReaction.File,
                        FileComment = fileCommentReaction.FileComment,
                        RawData = inboundMessage.RawData,
                        Reaction = inboundMessage.Reaction,
                        ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                    });
            }

            return RaiseReactionReceived(
                new SlackUnknownReaction
                {
                    User = GetMessageUser(inboundMessage.User),
                    Timestamp = inboundMessage.Timestamp,
                    RawData = inboundMessage.RawData,
                    Reaction = inboundMessage.Reaction,
                    ReactingToUser = GetMessageUser(inboundMessage.ReactingToUser)
                });
        }

        private Task HandleGroupJoined(GroupJoinedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub();
            _connectedHubs[channelId] = hub;

            return RaiseChatHubJoined(hub);
        }

        private Task HandleChannelJoined(ChannelJoinedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub();
            _connectedHubs[channelId] = hub;

            return RaiseChatHubJoined(hub);
        }

        private Task HandleDmJoined(DmChannelJoinedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub(_userCache.Values.ToArray());
            _connectedHubs[channelId] = hub;

            return RaiseChatHubJoined(hub);
        }

        private Task HandleUserJoined(UserJoinedMessage inboundMessage)
        {
            SlackUser slackUser = inboundMessage.User.ToSlackUser();
            _userCache[slackUser.Id] = slackUser;

            return RaiseUserJoined(slackUser);
        }

        private Task HandlePong(PongMessage inboundMessage)
        {
            _pingPongMonitor.Pong();
            return RaisePong(inboundMessage.Timestamp);
        }

        private Task HandleChannelCreated(ChannelCreatedMessage inboundMessage)
        {
            string channelId = inboundMessage?.Channel?.Id;
            if (channelId == null) return Task.CompletedTask;

            var hub = inboundMessage.Channel.ToChatHub();
            _connectedHubs[channelId] = hub;
            
            var slackChannelCreated = new SlackChannelCreated
            {
                Id = channelId,
                Name = inboundMessage.Channel.Name,
                Creator = GetMessageUser(inboundMessage.Channel.Creator)
            };
            return RaiseOnChannelCreated(slackChannelCreated);
        }

        private async Task HandlePresenceChange(PresenceChangeMessage inboundMessage)
        {
            if (!string.IsNullOrEmpty(inboundMessage.User))
                await RaiseOnPresenceChange(inboundMessage.User, inboundMessage.Presence);

			foreach (var user in inboundMessage.Users)
			{
                await RaiseOnPresenceChange(inboundMessage.User, inboundMessage.Presence);
            }
        }

        public event MessageReceivedEventHandler OnMessageReceived;
        private async Task RaiseMessageReceived(SlackMessage message)
        {
            var e = OnMessageReceived;
            if (e != null)
            {
                try
                {
                    await e(message);
                }
                catch (Exception)
                {

                }
            }
        }

        public event ReactionReceivedEventHandler OnReaction;
        private async Task RaiseReactionReceived(ISlackReaction reaction)
        {
            var e = OnReaction;
            if (e != null)
            {
                try
                {
                    await e(reaction);
                }
                catch (Exception)
                {

                }
            }
        }

        public event ChatHubJoinedEventHandler OnChatHubJoined;
        private async Task RaiseChatHubJoined(SlackChatHub hub)
        {
            var e = OnChatHubJoined;
            if (e != null)
            {
                try
                {
                    await e(hub);
                }
                catch
                {
                }
            }
        }

        public event UserJoinedEventHandler OnUserJoined;
        private async Task RaiseUserJoined(SlackUser user)
        {
            var e = OnUserJoined;
            try
            {
                if (e != null)
                {
                    await e(user);
                }
            }
            catch
            {
            }
        }

        public event PongEventHandler OnPong;
        private async Task RaisePong(DateTime timestamp)
        {
            var e = OnPong;
            if (e != null)
            {
                try
                {
                    await e(timestamp);
                }
                catch
                {
                }
            }
        }

        public event ChannelCreatedHandler OnChannelCreated;
        private async Task RaiseOnChannelCreated(SlackChannelCreated chatHub)
        {
            var e = OnChannelCreated;
            if (e != null)
            {
                try
                {
                    await e(chatHub);
                }
                catch
                {
                }
            }
        }

        public event PresenceChangeHandler OnPresenceChange;
        private async Task RaiseOnPresenceChange(string slackId, string presence)
        {
            var e = OnPresenceChange;
            if (e != null)
            {
                try
                {
                    await e(slackId, presence);
                }
                catch
                {
                }
            }
        }
        //TODO: USER JOINED EVENT HANDLING
    }
}
