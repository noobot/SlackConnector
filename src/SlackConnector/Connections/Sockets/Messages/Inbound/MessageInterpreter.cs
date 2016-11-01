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
                var message = JsonConvert.DeserializeObject<InboundMessage>(json);

                switch (message.MessageType)
                {
                    case MessageType.Group_Joined:
                        message = JsonConvert.DeserializeObject<GroupJoinedMessage>(json);
                        break;
                    case MessageType.Channel_Joined:
                        message = JsonConvert.DeserializeObject<ChannelJoinedMessage>(json);
                        break;
                    default:
                        message = GetInboundMessage(json);
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

        private static ChatMessage GetInboundMessage(string json)
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