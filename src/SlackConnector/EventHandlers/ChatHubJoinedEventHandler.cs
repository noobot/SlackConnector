using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.EventHandlers
{
    public delegate Task ChatHubJoinedEventHandler(SlackChatHub chatHub);
}
