using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.MessageActions
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
					var inboundBA = jObject.ToObject<BlockActionPayload>();
					var inboundActions = new List<InboundBlockAction>();
					foreach (var jActionItem in jObject.actions)
					{
						InboundBlockAction action = null;
						action = jActionItem.type == "button" ?
							jActionItem.ToObject<InboundBlockAction>()
							: jActionItem.ToObject<SelectInboundBlockAction>();
						inboundActions.Add(action);
					}
					inboundBA.Actions = inboundActions;
					return inboundBA;
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
