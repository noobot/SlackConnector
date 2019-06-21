using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Elements;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public class SectionBlock : BlockBase
	{
		public SectionBlock() : base("section")
		{
			this.Fields = new List<TextObject>();
		}

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ElementBase Accessory { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public TextObject Text { get; set; }

		public IList<TextObject> Fields { get; set; }
	}
}
