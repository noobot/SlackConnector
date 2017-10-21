using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.EventHandlers
{
    public delegate Task ReactionReceivedEventHandler(ISlackReaction message);
}