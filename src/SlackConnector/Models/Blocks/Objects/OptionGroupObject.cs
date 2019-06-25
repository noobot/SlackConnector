using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Objects
{
	public class OptionGroupObject
	{
		public OptionGroupObject(string label)
		{
			if (label == null)
			{
				throw new ArgumentNullException(nameof(label));
			}

			this.Label = new TextObject(label, TextObjectType.PlainText);
			this.Options = new List<OptionObject>();
		}

		[JsonProperty(PropertyName = "label")]
		public TextObject Label { get; set; }

		[JsonProperty(PropertyName = "options")]
		public IList<OptionObject> Options { get; set; }
	}
}
