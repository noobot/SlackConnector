using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackLibrary.EventHandlers
{
	public delegate Task PresenceChangeHandler(string slackId, string presence);
}
