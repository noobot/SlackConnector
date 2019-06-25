using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
	public class DialogSubmissionPayload : CommonActionPayload
	{
		[JsonProperty("callback_id")]
		public string CallbackId { get; set; }

		[JsonProperty("action_ts")]
		public string ActionTimestamp { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("submission")]
		public dynamic Submission { get; set; }
	}
}
