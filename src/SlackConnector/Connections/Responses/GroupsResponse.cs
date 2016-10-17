using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class GroupsResponse : StandardResponse
    {
         public Group[] Groups { get; set; }
    }
}