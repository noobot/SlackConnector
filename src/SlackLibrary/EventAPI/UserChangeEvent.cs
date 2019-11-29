using SlackLibrary.Connections.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.EventAPI
{
    public class UserChangeEvent : InboundEvent
    {
		public User User { get; set; }
    }
}
