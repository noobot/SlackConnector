using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.EventAPI
{
    public interface IEventInterpreter
    {
		InboundOuterEvent InterpretEvent(string json);
	}
}
