using System.Collections.Generic;

namespace SlackConnector.Models
{
    public class BotMessage
    {
        public IList<SlackAttachment> Attachments { get; set; }
        public SlackChatHub ChatHub { get; set; }
        public string Text { get; set; }
		public string ThreadTimestamp { get; set; }
		public string UserName { get; set; }
		public string IconUrl { get; set; }

        public BotMessage()
        {
            Attachments = new List<SlackAttachment>();
        }
    }
}