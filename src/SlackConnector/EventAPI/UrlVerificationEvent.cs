using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
    public class UrlVerificationEvent : InboundOuterEvent
    {
		[JsonProperty("challenge")]
		public string Challenge { get; set; }
    }
}
