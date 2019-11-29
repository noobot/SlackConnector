using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Responses
{
    internal class GroupsResponse : DefaultStandardResponse
	{
         public Group[] Groups { get; set; }
    }
}