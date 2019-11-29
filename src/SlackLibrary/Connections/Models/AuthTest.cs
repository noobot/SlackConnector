using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Connections.Models
{
	public class AuthTest
	{
		public string Url { get; set; }

		public string Team { get; set; }

		public string User { get; set; }

		public string TeamId { get; set; }

		public string UserId { get; set; }
	}
}
