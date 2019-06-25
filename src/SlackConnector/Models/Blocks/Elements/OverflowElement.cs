using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class OverflowElement : InteractiveElement
	{
		public OverflowElement(string actionId) : base(actionId, "overflow")
		{
			this.Options = new List<OptionObject>();
		}

		[JsonProperty(PropertyName = "options")]
		public IList<OptionObject> Options { get; set; }
	}
}
