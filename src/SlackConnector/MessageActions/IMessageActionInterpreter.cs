using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.MessageActions
{
    public interface IMessageActionInterpreter
    {
		CommonActionPayload InterpretMessageAction(string json);
    }
}
