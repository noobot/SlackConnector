using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Elements;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public class SectionBlock : BlockBase
	{
		public SectionBlock() : base("section")
		{
		}

		[JsonProperty(PropertyName = "accessory", NullValueHandling = NullValueHandling.Ignore)]
		public ElementBase Accessory { get; set; }

		[JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
		public TextObject Text { get; set; }

		[JsonProperty(PropertyName = "fields", NullValueHandling = NullValueHandling.Ignore)]
		public IList<TextObject> Fields { get; set; }

		public SectionBlock AddMarkdownTextField(string text)
		{
			this.Fields = this.Fields ?? new List<TextObject>();
			this.Fields.Add(new TextObject(text, TextObjectType.Markdown));
			return this;
		}

		public SectionBlock AddPlainTextField(string text)
		{
			this.Fields = this.Fields ?? new List<TextObject>();
			this.Fields.Add(new TextObject(text, TextObjectType.PlainText));
			return this;
		}

		public SectionBlock AddTextField(TextObject textObject)
		{
			this.Fields = this.Fields ?? new List<TextObject>();
			this.Fields.Add(textObject);
			return this;
		}
	}
}
