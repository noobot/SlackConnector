using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class UsersSelectElement : InteractiveElement
	{
		public const string ElementName = "users_select";
		public UsersSelectElement(string actionId, string placeholder) : base(actionId, ElementName)
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		[JsonProperty(PropertyName = "placeholder")]
		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "initial_user", NullValueHandling = NullValueHandling.Ignore)]
		public string InitialUser { get; set; }
	}
}
