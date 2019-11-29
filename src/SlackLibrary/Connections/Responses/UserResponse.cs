using SlackLibrary.Connections.Models;

namespace SlackLibrary.Connections.Responses
{
	public class IdentityResponse : DefaultStandardResponse
	{
		public UserIdentity User { get; set; }

		public TeamIdentity Team { get; set; }
	}

	public class UserResponse : DefaultStandardResponse
	{
		public User User { get; set; }
	}

	public class UserCollectionResponse : CursoredResponse
    {
         public User[] Members { get; set; }
    }
}