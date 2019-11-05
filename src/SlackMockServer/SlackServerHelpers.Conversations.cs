using SlackConnector.Connections.Clients.Conversation;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SlackMockServer
{
    public static partial class SlackServerHelpers
    {
		public static SlackServer MockConversationList(this SlackServer server, 
			string cursor = null, bool? excludeArchived = null, int? limit = null, string types = null,
			params ConversationChannel[] conversations)
		{
			var givenRequest = Request.Create().WithPath(FlurlConversationClient.CONVERSATION_LIST_PATH);
			if (cursor != null)
				givenRequest = givenRequest.WithParam("cursor", cursor);
			if (excludeArchived != null)
				givenRequest = givenRequest.WithParam("exclude_archived", excludeArchived.ToString().ToLowerInvariant());
			if (limit != null)
				givenRequest = givenRequest.WithParam("limit", limit.Value.ToString());
			if (types != null)
				givenRequest = givenRequest.WithParam("types", types);

			server.HttpServer.Given(givenRequest)
				.RespondWith(Response.Create().WithCallback(request =>
				{
					return new WireMock.ResponseMessage()
					{
						StatusCode = 200,
						BodyData = new WireMock.Util.BodyData()
						{
							DetectedBodyType = WireMock.Util.BodyType.Json,
							BodyAsJson = new ConversationCollectionReponse()
							{
								Ok = true,
								Channels = conversations
							}
						}
					};
				}));

			return server;
		}

		public static SlackServer MockDefaultConversationOpen(this SlackServer server)
		{
			string GetChannelId(RequestMessage request)
			{
				var users = string.Concat(request.GetParameterValuesFromPostOrGet("users"));

				return $"DM{users}";
			}

			server.HttpServer.Given(Request.Create().WithPath(FlurlConversationClient.CONVERSATION_OPEN_PATH))
				.RespondWith(Response.Create().WithCallback(request =>
				{
					return new WireMock.ResponseMessage()
					{
						StatusCode = 200,
						BodyData = new WireMock.Util.BodyData()
						{
							DetectedBodyType = WireMock.Util.BodyType.Json,
							BodyAsJson = new ConversationResponse()
							{
								Ok = true,
								Channel = new ConversationChannel() { Id = GetChannelId(request) }
							}
						}
					};
				}));

			return server;
		}
	}
}
