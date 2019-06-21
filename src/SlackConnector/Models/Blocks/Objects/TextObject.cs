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

	public class TextObject
	{
		public TextObject()
		{
			this.Type = TextObjectType.Markdown;
			this.Emoji = true;
		}

		public TextObject(string text) : this()
		{
			Text = text;
		}

		public TextObject(string text, string type) : this(text)
		{
			Type = type;
		}

		public string Text { get; set; }

		public string Type { get; set; }

		public bool Emoji { get; set; }

		public bool Verbatim { get; set; }
	}
}
