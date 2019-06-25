using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.EventAPI
{
    public interface IEventInterpreter
    {
		InboundOuterEvent InterpretEvent(string json);
	}
}
