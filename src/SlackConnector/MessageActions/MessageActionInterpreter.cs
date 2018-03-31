using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public class MessageActionInterpreter : IMessageActionInterpreter
	{
		public InboundMessageAction InterpretMessageAction(string json)
		{
			dynamic jObject = JObject.Parse(json);
			InboundMessageAction inboundMA = jObject.ToObject<InboundMessageAction>();
			var list = new List<InboundMessageAction.InboundAction>();
			foreach (var jActionItem in jObject.actions)
			{
				InboundMessageAction.InboundAction action = null;
				action = jActionItem.type == "button" ? 
					jActionItem.ToObject< InboundMessageAction.InboundButtonAction>()
					: jActionItem.ToObject<InboundMessageAction.InboundOptionsAction>();
				list.Add(action);
			}
			inboundMA.Actions = list.ToArray();
			inboundMA.RawJson = json;
			return inboundMA;
		}
	}
}
