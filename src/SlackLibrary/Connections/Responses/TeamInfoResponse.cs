using Newtonsoft.Json;
using SlackLibrary.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Responses
{
    public class TeamInfoResponse : DefaultStandardResponse
	{
		[JsonProperty("team")]
		public Team Team { get; set; }
    }
}
