using System;
using System.Collections.Generic;
using System.Text;

namespace SlackLibrary.MessageActions
{
    public interface IMessageActionInterpreter
    {
		CommonActionPayload InterpretMessageAction(string json);
    }
}
