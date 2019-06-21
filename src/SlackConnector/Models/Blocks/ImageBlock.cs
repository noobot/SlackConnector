using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks
{
	public class ImageBlock : BlockBase
	{
		public ImageBlock() : base("image")
		{
		}

		[JsonProperty(PropertyName = "image_url")]
		public string ImageUrl { get; set; }

		[JsonProperty(PropertyName = "alt_text", NullValueHandling = NullValueHandling.Ignore)]
		public string AltText { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public TextObject Title { get; set; }
	}
}
