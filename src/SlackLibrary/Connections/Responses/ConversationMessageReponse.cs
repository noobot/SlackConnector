using SlackLibrary.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Responses
{
	public class ConversationMessageReponse : CursoredResponse
	{
		public ConversationMessage[] Messages { get; set; }
    }
}
