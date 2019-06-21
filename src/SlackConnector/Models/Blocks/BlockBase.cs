using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public abstract class BlockBase
	{
		public BlockBase(string type)
		{
			Type = type;
		}

		[JsonProperty(PropertyName = "type")]
		public string Type { get; protected set; }

		[JsonProperty(PropertyName = "block_id", NullValueHandling = NullValueHandling.Ignore)]
		public string BlockId { get; set; }
	}
}
