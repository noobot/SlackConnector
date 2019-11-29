using Newtonsoft.Json;
using SlackLibrary.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.EventAPI
{
    public class TeamDomainChangeEvent : InboundEvent
    {
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("domain")]
		public string Domain { get; set; }
	}

	public class TeamJoinEvent : InboundEvent
	{
		[JsonProperty("user")]
		public User User { get; set; }
	}

	public class TeamRenameEvent : InboundEvent
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
