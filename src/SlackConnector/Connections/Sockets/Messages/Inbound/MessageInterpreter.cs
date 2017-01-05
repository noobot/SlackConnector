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
            try
            {
                MessageType messageType = ParseMessageType(json);

                InboundMessage message;
                switch (messageType)
                {
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
                    default:
                        message = GetChatMessage(json);
                        break;
                }

                message.RawData = json;

                return message;
            }
            catch (Exception ex)
            {
                if (SlackConnector.LoggingLevel > ConsoleLoggingLevel.None)
                {
                    _logger.LogError($"Unable to parse message: '{json}'");
                    _logger.LogError(ex.ToString());
                }
            }

            return null;
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
    }
}