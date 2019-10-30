using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Clients.Conversation;
using SlackConnector.Connections.Clients.Users;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SlackMockServer
{
	public static partial class SlackServerHelpers
	{
		public static string GenerateTimestamp()
		{
			var sTicks = DateTime.Now.Ticks.ToString();
			var ticks = sTicks.Substring(sTicks.Length - 10, 10);
			return ticks.Substring(0, 5) + "." + ticks.Substring(5, 5);
		}

		public static IEnumerable<string> GetParameterValuesFromPostOrGet(this RequestMessage request, string key)
		{
			IEnumerable<string> values = request.GetParameter(key);

			if (values is null || !values.Any())
			{
				if (request.Body is null)
					return Enumerable.Empty<string>();

				var qs = HttpUtility.ParseQueryString(request.Body.TrimStart('?'));
				values = qs[key]?.Split(',');
			}

			return values ?? Enumerable.Empty<string>();
		}

		public static string GetParameterValueFromPostOrGet(this RequestMessage request, string key)
		{
			IEnumerable<string> values = request.GetParameter(key);

			if (values is null || !values.Any())
			{
				var qs = HttpUtility.ParseQueryString(request.Body.TrimStart('?'));
				return qs[key];
			}

			return values?.FirstOrDefault();
		}

		private static string GetUserIdFromChannel(this RequestMessage request)
		{
			var channel = request.GetChannel();
			if (channel.StartsWith("DM"))
				return channel.Substring(2);
			return null;
		}

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
