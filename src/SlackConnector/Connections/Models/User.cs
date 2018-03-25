using Newtonsoft.Json;
using SlackConnector.Serialising;
using System;

namespace SlackConnector.Connections.Models
{
    public class User : Detail
    {
        public bool Deleted { get; set; }

        public Profile Profile { get; set; }

        [JsonProperty("tz_offset")]
        public long TimeZoneOffset { get; set; }

        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("is_guest")]
        public bool IsGuest { get; set; }

        [JsonProperty("presence")]
        public string Presence { get; set; }

		[JsonProperty("team_id")]
		public string TeamId { get; set; }

		[JsonProperty("color")]
		public string Color { get; set; }

		[JsonProperty("real_name")]
		public string RealName { get; set; }

		[JsonProperty("tz")]
		public string TimeZone { get; set; }

		[JsonProperty("tz_label")]
		public string TimeZoneLabel { get; set; }

		[JsonProperty("is_owner")]
		public bool IsOwner { get; set; }

		[JsonProperty("is_primary_owner")]
		public bool IsPrimaryOwner { get; set; }

		[JsonProperty("is_restricted")]
		public bool IsRestricted { get; set; }

		[JsonProperty("is_ultra_restricted")]
		public bool IsUltraRestricted { get; set; }

		[JsonProperty("updated")]
		[JsonConverter(typeof(SecondEpochConverter))]
		public DateTime Updated { get; set; }

		[JsonProperty("is_app_user")]
		public bool IsAppUser { get; set; }

		[JsonProperty("is_stranger")]
		public bool IsStranger { get; set; }

		[JsonProperty("has_2fa")]
		public bool Has2Fa { get; set; }

		[JsonProperty("locale")]
		public string Locale { get; set; }
	}
}