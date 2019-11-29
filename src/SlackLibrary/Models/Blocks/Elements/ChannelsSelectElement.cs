using Newtonsoft.Json;
using SlackLibrary.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Models.Blocks.Elements
{
	public class ChannelsSelectElement : InteractiveElement
	{
		public const string ElementName = "channels_select";
		public ChannelsSelectElement(string actionId, string placeholder) : base(actionId, ElementName)
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		[JsonProperty(PropertyName = "placeholder")]
		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "initial_channel", NullValueHandling = NullValueHandling.Ignore)]
		public string InitialChannel { get; set; }
	}
}
