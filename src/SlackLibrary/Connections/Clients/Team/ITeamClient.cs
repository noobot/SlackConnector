using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackLibrary.Connections.Clients.Team
{
    public interface ITeamClient
    {
		Task<Models.Team> GetTeamInfo(string slackKey);
    }
}
