using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Objects
{
	public static class TextObjectType
	{
		public const string Markdown = "mrkdwn";

		public const string PlainText = "plain_text";
	}

	public class TextObject : IContextElement
	{
		public TextObject()
		{
			this.Type = TextObjectType.Markdown;
		}

		public TextObject(string text) : this()
		{
			Text = text;
		}

		public TextObject(string text, string type) : this(text)
		{
			Type = type;
			if (type == TextObjectType.PlainText)
				this.Emoji = true;
		}

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "emoji", NullValueHandling = NullValueHandling.Ignore)]
		public bool? Emoji { get; set; }

		[JsonProperty(PropertyName = "verbatim", NullValueHandling = NullValueHandling.Ignore)]
		public bool? Verbatim { get; set; }
	}
}
