using Newtonsoft.Json;
using SlackConnector.Models;
using SlackConnector.Models.Blocks;
using SlackConnector.Serialising;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WireMock;

namespace SlackMockServer
{
	public static class RequestMessagePredicates
	{
		public static string GetChannel(this RequestMessage request)
		{
			return request.GetParameterValueFromPostOrGet("channel");
		}

		public static string GetText(this RequestMessage request)
		{
			return request.GetParameterValueFromPostOrGet("text");
		}


		public static bool IsTextEqualTo(this RequestMessage requestMessage, string text)
		{
			return requestMessage.GetText() == text;
		}

		public static bool DoesTextContains(this RequestMessage requestMessage, string text)
		{
			return requestMessage.GetText().Contains(text);
		}

		public static bool IsTokenEqualTo(this RequestMessage requestMessage, string token)
		{
			return requestMessage.GetParameterValueFromPostOrGet("token") == token;
		}

		public static bool IsChannelEqualTo(this RequestMessage requestMessage, string channelId)
		{
			return requestMessage.GetParameterValueFromPostOrGet("channel") == channelId;
		}

		public static bool DoesUsernameContains(this RequestMessage requestMessage, string username)
		{
			return (requestMessage.GetParameterValueFromPostOrGet("username")?? string.Empty).Contains(username);
		}

		public static bool IsAsUserFalse(this RequestMessage requestMessage)
		{
			return requestMessage.GetParameterValueFromPostOrGet("as_user").Equals("false", StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool IsAsUserTrue(this RequestMessage requestMessage)
		{
			return requestMessage.GetParameterValueFromPostOrGet("as_user").Equals("true", StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool IsTimestampEqualTo(this RequestMessage requestMessage, string ts)
		{
			return requestMessage.GetParameterValueFromPostOrGet("ts") == ts;
		}

		public static bool IsThreadTimestampEqualTo(this RequestMessage requestMessage, string ts)
		{
			return requestMessage.GetParameterValueFromPostOrGet("thread_ts") == ts;
		}

		public static bool IsThreadTimestampNotEmpty(this RequestMessage requestMessage)
		{
			return requestMessage.GetParameterValuesFromPostOrGet("thread_ts").Any(_ => !string.IsNullOrEmpty(_));
		}

		public static bool DoesAttachmentContains(this RequestMessage requestMessage, string text)
		{
			return requestMessage.GetParameterValueFromPostOrGet("attachments")?.Contains(text) ?? false;
		}

		public static bool DoesBlocksContains(this RequestMessage requestMessage, string text)
		{
			return requestMessage.GetParameterValueFromPostOrGet("blocks")?.Contains(text) ?? false;
		}

		public static SlackAttachment[] GetAttachments(this RequestMessage requestMessage)
		{
			var rawAttachments = requestMessage.GetParameterValueFromPostOrGet("attachments");

			var attachments = JsonConvert.DeserializeObject<SlackConnector.Models.SlackAttachment[]>(rawAttachments);

			return attachments;
		}

		public static IList<BlockBase> GetBlocks(this RequestMessage requestMessage)
		{
			var rawBlocks = requestMessage.GetParameterValueFromPostOrGet("blocks");

			var deserializer = new BlockDeserializer();

			var blocks = deserializer.Deserialize(rawBlocks);

			return blocks.ToList();
		}
	}
}
