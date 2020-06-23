using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Sockets.Messages.Outbound
{
	public class PresenceSubMessage : BaseMessage
	{
		[JsonProperty("ids")]
		public IEnumerable<string> Ids { get; set; }

		public PresenceSubMessage()
		{
			Type = "presence_sub";
		}
	}
}
