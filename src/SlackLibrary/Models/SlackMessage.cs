using SlackLibrary.Connections.Models;
using System.Collections.Generic;

namespace SlackLibrary.Models
{
    public class SlackMessage
    {
        public SlackChatHub ChatHub { get; set; }
        public bool MentionsBot { get; set; }
        public string RawData { get; set; }
        public string Text { get; set; }
        public SlackUser User { get; set; }
        public string Timestamp { get; set; }
		public string ThreadTimestamp { get; set; }
        public SlackMessageSubType MessageSubType { get; set; }

		public IEnumerable<SlackFile> Files { get; set; }
	}
}