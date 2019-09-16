using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public class ImageBlock : BlockBase
	{
		public const string BlockName = "image";
		public ImageBlock(string imageUrl, string altText) : base(BlockName)
		{
			ImageUrl = imageUrl;
			AltText = altText;
		}

		[JsonProperty(PropertyName = "image_url")]
		public string ImageUrl { get; set; }

		[JsonProperty(PropertyName = "alt_text", NullValueHandling = NullValueHandling.Ignore)]
		public string AltText { get; set; }

		[JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore)]
		public TextObject Title { get; set; }

		public ImageBlock WithTitle(string title)
		{
			this.Title = new TextObject(title, TextObjectType.PlainText);
			return this;
		}
	}
}
