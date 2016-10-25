using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class UsersResponse : StandardResponse
    {
         public User[] Members { get; set; }
    }
}