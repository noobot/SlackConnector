using SlackConnector.Connections.Clients.Auth;
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
		public static SlackServer MockDefaultOAuthAccess(this SlackServer server, OAuthAccessResponse response)
		{
			server.HttpServer.Given(Request.Create().WithPath(FlurlAuthClient.OAUTH_ACCESS)
				.UsingPost(WireMock.Matchers.MatchBehaviour.AcceptOnMatch))
				.RespondWith(Response.Create().WithCallback(request =>
				{
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

		public static SlackServer MockDefaultAuthTest(this SlackServer server, AuthTestResponse response)
		{
			server.HttpServer.Given(Request.Create().WithPath(FlurlAuthClient.AUTH_TEST_PATH)
				.UsingGet(WireMock.Matchers.MatchBehaviour.AcceptOnMatch))
				.RespondWith(Response.Create().WithCallback(request =>
				{
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
