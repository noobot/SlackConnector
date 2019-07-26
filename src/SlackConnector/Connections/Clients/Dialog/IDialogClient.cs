using SlackConnector.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackConnector.Connections.Clients.Dialog
{
	public interface IDialogClient
	{
		Task<MessageResponse> Open(string slackKey, Models.Dialog dialog, string triggerId);
	}
}
