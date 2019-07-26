using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Connections.Responses
{
	public class CursoredResponseMetadata
	{
		[JsonProperty("next_cursor")]
		public string NextCursor { get; set; }
	}

	public class CursoredResponse : StandardResponse<CursoredResponseMetadata>
	{
    }
}
