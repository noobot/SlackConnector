using Newtonsoft.Json;
using SlackConnector.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Responses
{
    public class TeamInfoResponse : DefaultStandardResponse
	{
		[JsonProperty("team")]
		public Team Team { get; set; }
    }
}
