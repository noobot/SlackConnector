using System;
using System.Net;
using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class MessageInterpreter : IMessageInterpreter
    {
        public InboundMessage InterpretMessage(string json)
        {
            InboundMessage message = null;

            try
            {
                message = JsonConvert.DeserializeObject<InboundMessage>(json);

                if (message != null)
                {
                    message.Channel = WebUtility.HtmlDecode(message.Channel);
                    message.User = WebUtility.HtmlDecode(message.User);
                    message.Text = WebUtility.HtmlDecode(message.Text);
                    message.Team = WebUtility.HtmlDecode(message.Team);
                    message.RawData = json;
                }

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

            return message;
        }
    }
}