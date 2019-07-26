using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
    internal class GroupsResponse : DefaultStandardResponse
	{
         public Group[] Groups { get; set; }
    }
}