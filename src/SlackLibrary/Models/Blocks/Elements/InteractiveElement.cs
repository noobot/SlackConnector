using Newtonsoft.Json;
using SlackLibrary.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Models.Blocks.Elements
{
	public abstract class InteractiveElement : ElementBase
	{
		public InteractiveElement(string actionId, string type) : base(type)
		{
			ActionId = actionId;
		}

		[JsonProperty(PropertyName = "action_id", NullValueHandling = NullValueHandling.Ignore)]
		public string ActionId { get; set; }

		[JsonProperty(PropertyName = "confirm", NullValueHandling = NullValueHandling.Ignore)]
		public ConfirmObject Confirm { get; set; }
	}
}
