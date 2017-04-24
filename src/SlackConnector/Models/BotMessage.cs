using System.Collections.Generic;

namespace SlackConnector.Models
{
    public class BotMessage
    {
        public IList<SlackAttachment> Attachments { get; set; }
        public SlackChatHub ChatHub { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// message timestamp for updating an existing message
        /// </summary>
        public string Ts { get; set; }

        public BotMessage()
        {
            Attachments = new List<SlackAttachment>();
        }
    }
}