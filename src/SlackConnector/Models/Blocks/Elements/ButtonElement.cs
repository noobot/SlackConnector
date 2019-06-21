using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class ButtonElement : InteractiveElement
	{
		public ButtonElement(string actionId) : base(actionId, "button")
		{
		}

		public string Url { get; set; }

		public string Value { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public SlackActionStyle? Style { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ConfirmObject Confirm { get; set; }
	}
}
