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
		public static SlackServer MockConversationList(this SlackServer server, params ConversationChannel[] conversations)
		{
			server.HttpServer.Given(Request.Create().WithPath(FlurlConversationClient.CONVERSATION_LIST_PATH))
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
