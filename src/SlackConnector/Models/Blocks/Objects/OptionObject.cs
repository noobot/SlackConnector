using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Objects
{
	public class OptionObject
	{
		public OptionObject(string text, string value)
		{
			Text = text ?? throw new ArgumentNullException(nameof(text));
			Value = value ?? throw new ArgumentNullException(nameof(value));

			if (text.Length > 75)
				throw new ArgumentException("Text length can't be greater than 75");
			if (value.Length > 75)
				throw new ArgumentException("Value length can't be greater than 75");
		}

		[JsonProperty(PropertyName = "text")]
		public string Text { get; }

		[JsonProperty(PropertyName = "value")]
		public string Value { get; }

		[JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get; }
	}
}
