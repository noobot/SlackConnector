using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.MessageActions
{
	public partial class ActionPayloadChannel
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public partial class ActionPayloadUser
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public partial class ActionPayloadTeam
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("domain")]
		public string Domain { get; set; }
	}
	public class CommonActionPayload
	{
		[JsonProperty("team")]
		public ActionPayloadTeam Team { get; set; }

		[JsonProperty("channel")]
		public ActionPayloadChannel Channel { get; set; }

		[JsonProperty("user")]
		public ActionPayloadUser User { get; set; }

		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("response_url")]
		public string ResponseUrl { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
