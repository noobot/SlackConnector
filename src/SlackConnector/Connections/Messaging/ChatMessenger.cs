using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.Connections.Messaging
{
    internal class ChatMessenger : IChatMessenger
    {
        private readonly IRestSharpFactory _restSharpFactory;

        public ChatMessenger(IRestSharpFactory restSharpFactory)
        {
            _restSharpFactory = restSharpFactory;
        }

        public Task PostMessage(string slackKey, string channel, string text, SlackAttachment[] attachments)
        {
            throw new System.NotImplementedException();
        }
    }
}