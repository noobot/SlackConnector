using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public partial class InboundMessageActionChannel
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public partial class InboundMessageActionUser
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public partial class InboundMessageActionTeam
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("domain")]
		public string Domain { get; set; }
	}
	public class InboundCommonMessageAction
	{
		[JsonProperty("team")]
		public InboundMessageActionTeam Team { get; set; }

		[JsonProperty("channel")]
		public InboundMessageActionChannel Channel { get; set; }

		[JsonProperty("user")]
		public InboundMessageActionUser User { get; set; }

		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("response_url")]
		public string ResponseUrl { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
