using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Models
{
	public class UserIdentity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class TeamIdentity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class Identity
	{
		public Identity(UserIdentity user, TeamIdentity team)
		{
			User = user;
			Team = team;
		}

		[JsonProperty("user")]
		public UserIdentity User { get; set; }

		[JsonProperty("team")]
		public TeamIdentity Team { get; set; }
	}
}
