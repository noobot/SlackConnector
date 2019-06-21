using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class ChannelsSelectElement : InteractiveElement
	{
		public ChannelsSelectElement(string actionId, string placeholder) : base(actionId, "channels_select")
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "initial_channel", NullValueHandling = NullValueHandling.Ignore)]
		public string InitialChannel { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ConfirmObject Confirm { get; set; }
	}
}
