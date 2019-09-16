using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Elements;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public interface IContextElement
	{
	}

	public class ContextBlock : BlockBase
	{
		public const string BlockName = "context";
		public ContextBlock() : base(BlockName)
		{
			this.Elements = new List<IContextElement>();
		}

		[JsonProperty(PropertyName = "elements")]
		public IList<IContextElement> Elements { get; set; }

		public ContextBlock AddMarkdownText(string text)
		{
			this.Elements.Add(new TextObject(text, TextObjectType.Markdown));
			return this;
		}

		public ContextBlock AddPlainText(string text)
		{
			this.Elements.Add(new TextObject(text, TextObjectType.PlainText));
			return this;
		}

		public ContextBlock AddImage(string imageUrl, string altText)
		{
			var img = new ImageElement(imageUrl, altText);
			this.Elements.Add(img);
			return this;
		}
	}
}
