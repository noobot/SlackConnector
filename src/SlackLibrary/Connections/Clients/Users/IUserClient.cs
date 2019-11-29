using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackLibrary.Connections.Clients.Users
{
    public interface IUserClient
    {
		Task<CursoredResponse<Models.User>> List(string slackKey, string cursor = null, int? limit = null);

		Task<ICollection<Models.User>> ListAll(string slackKey);

		Task<Models.User> Info(string slackKey, string userId, bool? includeLocale = null);

		Task<Models.Identity> Identity(string slackKey);
	}
}
