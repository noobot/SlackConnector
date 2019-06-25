using SlackConnector.Connections.Models;

namespace SlackConnector.Connections.Responses
{
	public class IdentityResponse : StandardResponse
	{
		public UserIdentity User { get; set; }

		public TeamIdentity Team { get; set; }
	}

	public class UserResponse : StandardResponse
	{
		public User User { get; set; }
	}

	public class UserCollectionResponse : CursoredResponse
    {
         public User[] Members { get; set; }
    }
}