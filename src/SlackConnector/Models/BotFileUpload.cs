using System.Collections.Generic;

namespace SlackConnector.Models
{
    public class BotFileUpload
    {
        public SlackChatHub ChatHub { get; set; }
        public string File { get; set; }
    }
}