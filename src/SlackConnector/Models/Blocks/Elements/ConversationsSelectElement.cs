using Newtonsoft.Json;
using SlackConnector.Models.Blocks.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models.Blocks.Elements
{
	public class ConversationsSelectElement : InteractiveElement
	{
		public const string ElementName = "conversations_select";
		public ConversationsSelectElement(string actionId, string placeholder) : base(actionId, ElementName)
		{
			this.Placeholder = new TextObject(placeholder, TextObjectType.PlainText);
		}

		[JsonProperty(PropertyName = "placeholder")]
		public TextObject Placeholder { get; set; }

		[JsonProperty(PropertyName = "initial_conversation", NullValueHandling = NullValueHandling.Ignore)]
		public string InitialConversation { get; set; }
	}
}
