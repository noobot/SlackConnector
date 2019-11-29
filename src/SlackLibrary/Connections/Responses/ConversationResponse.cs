using SlackLibrary.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Responses
{
	public class ConversationResponse : DefaultStandardResponse
	{
		public ConversationChannel Channel { get; set; }
	}

	public class ConversationCollectionReponse : CursoredResponse
	{
		public ConversationChannel[] Channels { get; set; }
	}
}
