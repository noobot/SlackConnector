using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Models
{
    public class SlackMessagePosted
    {
        public double TimeStamp { get; set; }

        public string Channel { get; set; }

        /// <summary>
        /// Message object, as it was parsed by Slack servers
        /// </summary>
        public MessageObjectResponse Message { get; set; }
    }
}
