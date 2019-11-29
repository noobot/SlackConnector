using System.Threading.Tasks;
using SlackLibrary.Models;

namespace SlackLibrary.EventHandlers
{
    public delegate Task UserJoinedEventHandler(SlackUser user);
}
