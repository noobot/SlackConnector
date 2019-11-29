using SlackLibrary.Connections.Clients.Team;
using SlackLibrary.Connections.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SlackMockServer
{
    public static partial class SlackServerHelpers
    {
		public static SlackServer MockDefaultTeamInfo(this SlackServer server, SlackLibrary.Connections.Models.Team team)
		{
			server.HttpServer.Given(Request.Create().WithPath(FlurlTeamClient.TEAM_INFO))
			.RespondWith(Response.Create().WithCallback(request =>
			{
				return new WireMock.ResponseMessage()
				{
					StatusCode = 200,
					BodyData = new WireMock.Util.BodyData()
					{
						DetectedBodyType = WireMock.Util.BodyType.Json,
						BodyAsJson = new TeamInfoResponse()
						{
							Ok = true,
							Team = team
						}
					}
				};
			}));

			return server;
		}

		public static SlackServer MockTeamInfo(this SlackServer server, string token, SlackLibrary.Connections.Models.Team team)
		{
			var givenRequest = Request.Create().WithPath(FlurlTeamClient.TEAM_INFO)
				.WithParam("token", token);

			server.HttpServer.Given(givenRequest)
			.RespondWith(Response.Create().WithCallback(request =>
			{
				return new WireMock.ResponseMessage()
				{
					StatusCode = 200,
					BodyData = new WireMock.Util.BodyData()
					{
						DetectedBodyType = WireMock.Util.BodyType.Json,
						BodyAsJson = new TeamInfoResponse()
						{
							Ok = true,
							Team = team
						}
					}
				};
			}));

			return server;
		}
	}
}
