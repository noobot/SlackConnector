using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlackConnector.Models
{
    public class SlackMessageDeleted
    {
        public string TimeStamp { get; set; }

        public string Channel { get; set; }
    }
}
