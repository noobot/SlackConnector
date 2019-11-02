using SlackConnector.Connections.Clients.Users;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SlackMockServer
{
	public static partial class SlackServerHelpers
	{
		public static SlackServer MockUserInfo(this SlackServer server, User respondWithUser)
		{

			server.HttpServer.Given(Request.Create().WithPath(FlurlUserClient.USERS_INFO_PATH)
				.WithParam("user", respondWithUser.Id))
				.RespondWith(Response.Create().WithCallback(request =>
				{
					var response = new UserResponse()
					{
						User = respondWithUser,
						Ok = true,
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
