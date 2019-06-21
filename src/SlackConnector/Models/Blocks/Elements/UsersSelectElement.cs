using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class UsersSelectElement : InteractiveElement
	{
		public UsersSelectElement(string actionId, string placeholder) : base(actionId, "users_select")
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "initial_user", NullValueHandling = NullValueHandling.Ignore)]
		public string InitialUser { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ConfirmObject Confirm { get; set; }
	}
}
