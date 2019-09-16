using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class ExternalSelectElement : InteractiveElement
	{
		public const string ElementName = "external_select";
		public ExternalSelectElement(string actionId, string placeholder) : base(actionId, ElementName)
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		[JsonProperty(PropertyName = "placeholder")]
		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "initial_option", NullValueHandling = NullValueHandling.Ignore)]
		public OptionObject InitialOption { get; set; }

		[JsonProperty(PropertyName = "min_query_length", NullValueHandling = NullValueHandling.Ignore)]
		public int? MinQueryLength { get; set; }
	}
}
