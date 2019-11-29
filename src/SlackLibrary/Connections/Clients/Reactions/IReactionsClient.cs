using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackLibrary.Connections.Clients.Reactions
{
	public interface IReactionsClient
	{
		Task Add(string slackKey, string name, string channel = null, string messageTs = null, string file = null, string fileComment = null);

		Task Remove(string slackKey, string name, string channel = null, string messageTs = null, string file = null, string fileComment = null);
	}
}
