using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class ConversationsSelectElement : InteractiveElement
	{
		public ConversationsSelectElement(string actionId, string placeholder) : base(actionId, "conversations_select")
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "initial_conversation", NullValueHandling = NullValueHandling.Ignore)]
		public string InitialConversation { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ConfirmObject Confirm { get; set; }
	}
}
