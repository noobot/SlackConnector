using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class GroupsResponse : StandardResponse
    {
         internal Group[] Groups { get; set; }
    }
}