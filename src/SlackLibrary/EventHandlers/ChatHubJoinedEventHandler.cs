using System.Threading.Tasks;
using SlackLibrary.Models;

namespace SlackLibrary.EventHandlers
{
    public delegate Task ChatHubJoinedEventHandler(SlackChatHub chatHub);
}
