using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.Models.Blocks.Objects
{
	public class OptionObject
	{
		public OptionObject()
		{
		}

		public OptionObject(string text, string value)
		{
			if (text is null)
				throw new ArgumentNullException(nameof(text));
			Text = new TextObject(text, TextObjectType.PlainText);
			Value = value ?? throw new ArgumentNullException(nameof(value));

			if (text.Length > 75)
				throw new ArgumentException("Text length can't be greater than 75");
			if (value.Length > 75)
				throw new ArgumentException("Value length can't be greater than 75");
		}

		[JsonProperty(PropertyName = "text")]
		public TextObject Text { get; set; }

		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; }

		[JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
		public string Url { get; protected set; }

		public OptionObject WithUrl(string url)
		{
			this.Url = url;
			return this;
		}
	}
}
