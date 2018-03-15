using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
	public class EventInterpreter : IEventInterpreter
	{
		private InboundOuterEvent CreateInboundCommonOuterEvent<T>(string json) where T : InboundEvent
		{
			var outerEvent = new InboundOuterCommonEvent();
			outerEvent.Event = JsonConvert.DeserializeObject<T>(json);
			return outerEvent;
		}

		public InboundOuterEvent InterpretEvent(string json)
		{
			InboundOuterEvent outerEvent = null;

			try
			{
				var outerEventType = ParseOuterEventType(json);
				switch (outerEventType)
				{
					case OuterEventType.event_callback:
						var eventType = ParseEventType(json);
						switch (eventType)
						{
							case EventType.app_mention:
								outerEvent = this.CreateInboundCommonOuterEvent<AppMentionEvent>(json);
								break;
							case EventType.app_uninstalled:
								outerEvent = this.CreateInboundCommonOuterEvent<InboundEvent>(json);
								break;
							case EventType.channel_archive:
								outerEvent = this.CreateInboundCommonOuterEvent<ChannelArchiveEvent>(json);
								break;
							case EventType.channel_created:
								outerEvent = this.CreateInboundCommonOuterEvent<ChannelCreatedEvent>(json);
								break;
							case EventType.channel_deleted:
								outerEvent = this.CreateInboundCommonOuterEvent<ChannelDeletedEvent>(json);
								break;
							case EventType.channel_history_changed:
								outerEvent = this.CreateInboundCommonOuterEvent<ChannelHistoryChangedEvent>(json);
								break;
							case EventType.channel_rename:
								outerEvent = this.CreateInboundCommonOuterEvent<ChannelRenameEvent>(json);
								break;
							case EventType.message:
							case EventType.message_dot_channels:
							case EventType.message_dot_groups:
							case EventType.message_dot_im:
							case EventType.message_dot_mpim:
								outerEvent = this.CreateInboundCommonOuterEvent<MessageEvent>(json);
								break;
							case EventType.reaction_added:
								outerEvent = this.CreateInboundCommonOuterEvent<ReactionEvent>(json);
								break;
							case EventType.reaction_removed:
								outerEvent = this.CreateInboundCommonOuterEvent<ReactionEvent>(json);
								break;
							case EventType.team_domain_change:
								outerEvent = this.CreateInboundCommonOuterEvent<TeamDomainChangeEvent>(json);
								break;
							case EventType.team_join:
								outerEvent = this.CreateInboundCommonOuterEvent<TeamJoinEvent>(json);
								break;
							case EventType.team_rename:
								outerEvent = this.CreateInboundCommonOuterEvent<TeamRenameEvent>(json);
								break;
							case EventType.user_change:
								outerEvent = this.CreateInboundCommonOuterEvent<UserChangeEvent>(json);
								break;
						}
						break;
					case OuterEventType.url_verification:
						outerEvent = JsonConvert.DeserializeObject<UrlVerificationEvent>(json);
						break;
					case OuterEventType.app_rate_limited:
						outerEvent = JsonConvert.DeserializeObject<AppRateLimitedEvent>(json);
						break;

				}
			}
			catch (Exception ex)
			{
				if (SlackConnector.LoggingLevel > ConsoleLoggingLevel.None)
				{
					//_logger.LogError($"Unable to parse message: '{json}'");
					//_logger.LogError(ex.ToString());
				}
			}

			outerEvent.RawData = json;
			return outerEvent;
		}

		private static OuterEventType ParseOuterEventType(string json)
		{
			var eventType = OuterEventType.Unknown;
			if (!string.IsNullOrWhiteSpace(json))
			{
				var messageJobject = JObject.Parse(json);
				Enum.TryParse(messageJobject["type"].Value<string>(), true, out eventType);
			}

			return eventType;
		}

		private static EventType ParseEventType(string json)
		{
			var eventType = EventType.Unknown;
			if (!string.IsNullOrWhiteSpace(json))
			{
				var messageJobject = JObject.Parse(json);
				Enum.TryParse(messageJobject["event"]["type"].Value<string>(), true, out eventType);
			}

			return eventType;
		}
	}
}
