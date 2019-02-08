using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
    public interface IMessageActionInterpreter
    {
		InboundCommonMessageAction InterpretMessageAction(string json);
    }
}
