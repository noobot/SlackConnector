using System.Net;
using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages
{
    internal class MessageInterpreter : IMessageInterpreter
    {
        public InboundMessage InterpretMessage(string json)
        {
            InboundMessage message = JsonConvert.DeserializeObject<InboundMessage>(json);

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
    }
}