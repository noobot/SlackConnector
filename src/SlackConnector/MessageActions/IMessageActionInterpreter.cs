using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
    public interface IMessageActionInterpreter
    {
		InboundMessageAction InterpretMessageAction(string json);
    }
}
