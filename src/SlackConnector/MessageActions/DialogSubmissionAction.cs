using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public class DialogSubmissionAction : InboundCommonMessageAction
	{
		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("submission")]
		public dynamic Submission { get; set; }
	}
}
