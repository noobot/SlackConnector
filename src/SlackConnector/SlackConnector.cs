using System.Threading.Tasks;
using SlackConnector.EventHandlers;

namespace SlackConnector
{
    public class SlackConnector : ISlackConnector
    {
        public Task Connect(string slackKey)
        {
            throw new System.NotImplementedException();
        }

        public void Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;

        public event MessageReceivedEventHandler MessageReceived;
    }
}