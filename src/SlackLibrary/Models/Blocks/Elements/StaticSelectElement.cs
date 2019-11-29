using Newtonsoft.Json;
using SlackLibrary.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Models.Blocks.Elements
{
	public class StaticSelectElement : InteractiveElement
	{
		public const string ElementName = "static_select";

		public StaticSelectElement() : base(null, ElementName)
		{
		}

		public StaticSelectElement(string actionId, string placeholder) : base(actionId, ElementName)
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
			this.Options = new List<OptionObject>();
		}

		[JsonProperty(PropertyName = "placeholder")]
		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "options")]
		public IList<OptionObject> Options { get; set; }

		[JsonProperty(PropertyName = "option_groups", NullValueHandling = NullValueHandling.Ignore)]
		public IList<OptionObject> OptionGroups { get; set; }

		[JsonProperty(PropertyName = "initial_option", NullValueHandling = NullValueHandling.Ignore)]
		public OptionObject InitialOption { get; set; }
	}
}
