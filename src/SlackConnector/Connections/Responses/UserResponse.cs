using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
	internal class UserResponse : StandardResponse
	{
		public User User { get; set; }
	}

	internal class UserCollectionResponse : CursoredResponse
    {
         public User[] Members { get; set; }
    }
}