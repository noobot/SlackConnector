using Newtonsoft.Json;

namespace SlackConnector.Sockets.Messages
{
    internal class MessageInterpreter : IMessageInterpreter
    {
        public InboundMessage InterpretMessage(string json)
        {
            return JsonConvert.DeserializeObject<InboundMessage>(json);
        }
    }
}