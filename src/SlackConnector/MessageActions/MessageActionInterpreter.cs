using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public class MessageActionInterpreter : IMessageActionInterpreter
	{
		public CommonActionPayload InterpretMessageAction(string json)
		{
			dynamic jObject = JObject.Parse(json);
			switch ((string)jObject.type)
			{
				case "dialog_submission":
					return jObject.ToObject<DialogSubmissionPayload>();
				case "block_actions":
					return jObject.ToObject<BlockActionPayload>();
				default:
					var inboundMA = jObject.ToObject<ActionPayload>();
					var list = new List<ActionPayload.ActionPayloadAction>();
					foreach (var jActionItem in jObject.actions)
					{
						ActionPayload.ActionPayloadAction action = null;
						action = jActionItem.type == "button" ?
							jActionItem.ToObject<ActionPayload.ActionPayloadButton>()
							: jActionItem.ToObject<ActionPayload.ActionPayloadOptions>();
						list.Add(action);
					}
					inboundMA.Actions = list.ToArray();
					inboundMA.RawJson = json;
					return inboundMA;
			}
		}
	}
}
