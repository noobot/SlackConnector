using SlackLibrary.Connections.Clients.Chat;
using SlackLibrary.Connections.Responses;
using SlackLibrary.EventAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SlackMockServer
{
	public static partial class SlackServerHelpers
	{
		public static SlackServer MockDefaultDeleteMessage(this SlackServer server)
		{
			server.HttpServer.Given(Request.Create().WithPath(FlurlChatClient.DELETE_MESSAGE_PATH))
				.RespondWith(Response.Create().WithCallback(request =>
				{
					return new WireMock.ResponseMessage()
					{
						StatusCode = 200,
						BodyData = new WireMock.Util.BodyData()
						{
							DetectedBodyType = WireMock.Util.BodyType.Json,
							BodyAsJson = new DefaultStandardResponse()
							{
								Ok = true,
							}
						}
					};
				}));

			return server;
		}

		public static SlackServer MockDefaultUpdateMessage(this SlackServer server)
		{
			server.HttpServer.Given(Request.Create().WithPath(FlurlChatClient.UPDATE_MESSAGE_PATH))
				.RespondWith(Response.Create().WithCallback(request =>
				{
					return new WireMock.ResponseMessage()
					{
						StatusCode = 200,
						BodyData = new WireMock.Util.BodyData()
						{
							DetectedBodyType = WireMock.Util.BodyType.Json,
							BodyAsJson = new DefaultStandardResponse()
							{
								Ok = true,
							}
						}
					};
				}));

			return server;
		}

		public static SlackServer MockDefaultSendMessage(this SlackServer server)
		{
			server.HttpServer.Given(Request.Create().WithPath(FlurlChatClient.SEND_MESSAGE_PATH))
				.RespondWith(Response.Create().WithCallback(request =>
				{
					var response = new MessageResponse()
					{
						Ok = true,
						Timestamp = SlackServerHelpers.GenerateTimestamp(),
						Channel = request.GetChannel(),
						Message = new MessageEvent()
						{
							Timestamp = SlackServerHelpers.GenerateTimestamp(),
							Text = request.GetText(),
							ThreadTimestamp = request.GetParameterValuesFromPostOrGet("thread_ts")?.FirstOrDefault(),
							Channel = request.GetChannel(),
							User = request.GetUserIdFromChannel(),
						}
					};

					return new WireMock.ResponseMessage()
					{
						StatusCode = 200,
						BodyData = new WireMock.Util.BodyData()
						{
							DetectedBodyType = WireMock.Util.BodyType.Json,
							BodyAsJson = response
						}
					};
				}));

			return server;
		}
	}
}
