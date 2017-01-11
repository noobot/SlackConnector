using System.IO;

namespace SlackConnector.Models
{
    public class BotStreamUpload
    {
        public SlackChatHub ChatHub { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }
    }
}
