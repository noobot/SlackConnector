using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackConnector.Logging;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class MessageInterpreter : IMessageInterpreter
    {
        private readonly ILogger _logger;

        public MessageInterpreter(ILogger logger)
        {
            _logger = logger;
        }

        public InboundMessage InterpretMessage(string json)
        {
            InboundMessage message = new UnknownMessage();

            try
            {
                var messageType = ParseMessageType(json);
                switch (messageType)
                {
                    case MessageType.Message:
                        message = GetChatMessage(json);
                        break;
                    case MessageType.Group_Joined:
                        message = JsonConvert.DeserializeObject<GroupJoinedMessage>(json);
                        break;
                    case MessageType.Channel_Joined:
                        message = JsonConvert.DeserializeObject<ChannelJoinedMessage>(json);
                        break;
                    case MessageType.Team_Join:
                        message = JsonConvert.DeserializeObject<UserJoinedMessage>(json);
                        break;
                    case MessageType.Im_Created:
                        message = JsonConvert.DeserializeObject<DmChannelJoinedMessage>(json);
                        break;
                    case MessageType.Pong:
                        message = JsonConvert.DeserializeObject<PongMessage>(json);
                        break;
                    case MessageType.Reaction_Added:
                        message = GetReactionMessage(json);
                        break;
                }
            }
            catch (Exception ex)
            {
                if (SlackConnector.LoggingLevel > ConsoleLoggingLevel.None)
                {
                    _logger.LogError($"Unable to parse message: '{json}'");
                    _logger.LogError(ex.ToString());
                }
            }

            message.RawData = json;
            return message;
        }

        private static MessageType ParseMessageType(string json)
        {
            MessageType messageType = MessageType.Unknown;

            if (!string.IsNullOrWhiteSpace(json))
            {
                JObject messageJobject = JObject.Parse(json);
                Enum.TryParse(messageJobject["type"].Value<string>(), true, out messageType);
            }

            return messageType;
        }

        private static ChatMessage GetChatMessage(string json)
        {
            var message = JsonConvert.DeserializeObject<ChatMessage>(json);

            if (message != null)
            {
                message.Channel = WebUtility.HtmlDecode(message.Channel);
                message.User = WebUtility.HtmlDecode(message.User);
                message.Text = WebUtility.HtmlDecode(message.Text);
                message.Team = WebUtility.HtmlDecode(message.Team);
            }

            return message;
        }

        private static ReactionMessage GetReactionMessage(string json)
        {
            var message = JsonConvert.DeserializeObject<ReactionMessage>(json);

            if (message != null)
            {
                JObject messageJobject = JObject.Parse(json);
                message.Channel = messageJobject["item"]["channel"].Value<string>();
                message.ReactingToTimestamp = messageJobject["item"]["ts"].Value<double>();
                message.RawData = json;
            }

            return message;
        }
    }
}