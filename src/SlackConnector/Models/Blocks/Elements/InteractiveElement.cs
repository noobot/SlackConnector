using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
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
