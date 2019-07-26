using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Responses
{
	public class IncomingWebhookResponse
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("channel")]
		public string Channel { get; set; }

		[JsonProperty("configuration_url")]
		public string ConfigurationUrl { get; set; }
	}

	public class BotResponse
	{
		[JsonProperty("bot_user_id")]
		public string BotUserId { get; set; }

		[JsonProperty("bot_access_token")]
		public string BotAccessToken { get; set; }
	}
	public class OAuthAccessResponse : DefaultStandardResponse
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("scope")]
		public string Scope { get; set; }

		[JsonProperty("team_name")]
		public string TeamName { get; set; }

		[JsonProperty("team_id")]
		public string TeamId { get; set; }

		[JsonProperty("app_id")]
		public string AppId { get; set; }

		[JsonProperty("app_user_id")]
		public string AppUserId { get; set; }

		[JsonProperty("incoming_webhook")]
		public IncomingWebhookResponse IncomingWebhook { get; set; }

		[JsonProperty("bot")]
		public BotResponse Bot { get; set; }
	}
}
