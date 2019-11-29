using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.EventAPI
{
	public class TokensRevokedInfo
	{
		[JsonProperty("oauth")]
		public string[] OAuth { get; set; }

		[JsonProperty("bot")]
		public string[] Bot { get; set; }
	}

	public class TokensRevokedEvent : InboundEvent
	{
		[JsonProperty("tokens")]
		public TokensRevokedInfo Tokens { get; set; }
	}
}
