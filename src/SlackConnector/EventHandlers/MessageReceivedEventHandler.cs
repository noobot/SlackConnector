using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.EventHandlers
{
    public delegate Task MessageReceivedEventHandler(SlackMessage message);
}