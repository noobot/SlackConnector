using SlackConnector.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
    public class UserChangeEvent : InboundEvent
    {
		public User User { get; set; }
    }
}
