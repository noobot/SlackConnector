using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Sockets.Messages.Outbound
{
	public class PresenceQueryMessage : BaseMessage
	{
		[JsonProperty("ids")]
		public IEnumerable<string> Ids { get; set; }

		public PresenceQueryMessage()
		{
			Type = "presence_query";
		}
	}
}
