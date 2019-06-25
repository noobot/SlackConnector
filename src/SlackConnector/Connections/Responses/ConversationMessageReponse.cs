using SlackConnector.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Responses
{
	public class ConversationMessageReponse : CursoredResponse
	{
		public ConversationMessage[] Messages { get; set; }
    }
}
