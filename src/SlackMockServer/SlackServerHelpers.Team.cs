using SlackConnector.Connections.Clients.Team;
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
		public static SlackServer MockDefaultTeamInfo(this SlackServer server, SlackConnector.Connections.Models.Team team)
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
	}
}
