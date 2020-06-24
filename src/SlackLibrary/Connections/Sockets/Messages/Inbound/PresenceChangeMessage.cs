using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlackLibrary.Connections.Sockets.Messages.Inbound
{
	public class PresenceChangeMessage : InboundMessage
	{
		public PresenceChangeMessage() : base()
		{
			MessageType = MessageType.Presence_Change;
			this.Users = Enumerable.Empty<string>();
		}

		public string User { get; set; }

		public IEnumerable<string> Users { get; set; }

		public string Presence { get; set; }
	}
}
