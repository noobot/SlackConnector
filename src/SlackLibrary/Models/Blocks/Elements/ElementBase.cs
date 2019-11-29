using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Models.Blocks.Elements
{
	public abstract class ElementBase
	{
		public ElementBase(string type)
		{
			Type = type;
		}

		[JsonProperty(PropertyName = "type")]
		public string Type { get; protected set; }
	}
}
