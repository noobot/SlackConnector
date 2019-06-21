using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class ImageElement : ElementBase, IContextElement
	{
		public ImageElement(string imageUrl, string altText) : base("image")
		{
			ImageUrl = imageUrl;
			AltText = altText;
		}

		[JsonProperty(PropertyName = "image_url")]
		public string ImageUrl { get; set; }

		[JsonProperty(PropertyName = "alt_text", NullValueHandling = NullValueHandling.Ignore)]
		public string AltText { get; set; }
	}
}
