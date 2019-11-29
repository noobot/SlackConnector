using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Models
{
    public class Team : Detail
    {
		public class TeamIcon
		{
			[JsonProperty("image_34")]
			public string Image34 { get; set; }

			[JsonProperty("image_44")]
			public string Image44 { get; set; }

			[JsonProperty("image_68")]
			public string Image68 { get; set; }

			[JsonProperty("image_88")]
			public string Image88 { get; set; }

			[JsonProperty("image_102")]
			public string Image102 { get; set; }

			[JsonProperty("image_132")]
			public string Image132 { get; set; }

			[JsonProperty("image_default")]
			public bool ImageDefault { get; set; }
		}

		[JsonProperty("domain")]
		public string Domain { get; set; }

		[JsonProperty("email_domain")]
		public string EmailDomain { get; set; }

		[JsonProperty("enterprise_id")]
		public string EnterpriseId { get; set; }

		[JsonProperty("enterprise_name")]
		public string EnterpriseName { get; set; }

		[JsonProperty("icon")]
		public TeamIcon Icon { get; set; }
	}
}
