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

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ConfirmObject Confirm { get; set; }

		public IList<OptionObject> Options { get; set; }
	}
}
