using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class MessageInterpreter : IMessageInterpreter
    {
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
                    default:
                        message = GetChatMessage(json);
                        break;
                }

                message.RawData = json;

                return message;
            }
            catch (Exception ex)
            {
                if (SlackConnector.LoggingLevel == ConsoleLoggingLevel.FatalErrors)
                {
                    Console.WriteLine($"Unable to parse message: {json}");
                    Console.WriteLine(ex);
                }
            }

            return null;
        }

        private static MessageType ParseMessageType(string json)
        {
            var messageJobject = JObject.Parse(json);
            MessageType messageType;
            if (!Enum.TryParse(messageJobject["type"].Value<string>(), true, out messageType))
            {
                messageType = MessageType.Unknown;
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