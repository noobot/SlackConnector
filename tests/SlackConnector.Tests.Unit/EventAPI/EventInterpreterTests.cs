using SlackConnector.EventAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SlackConnector.Tests.Unit.EventAPI
{
    public class EventInterpreterTests
    {
		string content = "{\"token\":\"KgXyu8s38d421Ztz3tIuCmo6\",\"team_id\":\"T8SCBCK19\",\"api_app_id\":\"A9NNW6FML\",\"event\":{\"type\":\"message\",\"user\":\"U9QHV0L7K\",\"text\":\"Hey beautiful, we've got a new question !\",\"bot_id\":\"B9QHV0L65\",\"attachments\":[{\"author_name\":\"puzzledev\",\"text\":\"Hey beautiful, we've got a new question !\",\"id\":1,\"author_icon\":\"https://secure.gravatar.com/avatar/129f350e2613a505812a3b25d9d57fde.jpg?s=512&d=https%3A%2F%2Fa.slack-edge.com%2F7fa9%2Fimg%2Favatars%2Fava_0007-512.png\",\"actions\":[{\"id\":\"1\",\"name\":\"answerers\",\"text\":\"Choose answerers\",\"type\":\"button\",\"value\":\"answerers\",\"style\":\"default\"}],\"fallback\":\"Hey beautiful, we've got a new question !\"}],\"ts\":\"1522100146.000254\",\"channel\":\"C9VMFL2UT\",\"event_ts\":\"1522100146.000254\"},\"type\":\"event_callback\",\"event_id\":\"Ev9VMU5C0G\",\"event_time\":1522100146,\"authed_users\":[\"U9QHV0L7K\"]}";

		[Fact]
		public void should_deserialize_to_message_when_given_event()
		{
			var interpreter = new EventInterpreter();
			var obj = interpreter.InterpretEvent(content);
			Assert.True(obj is InboundOuterCommonEvent, "object is not of type InboundOuterCommonEvent");
			Assert.True((obj as InboundOuterCommonEvent).Event is MessageEvent, "object is not of type MessageEvent");
			Assert.Equal((obj as InboundOuterCommonEvent).GetEvent<MessageEvent>().BotId, "B9QHV0L65");
		}

	}
}
