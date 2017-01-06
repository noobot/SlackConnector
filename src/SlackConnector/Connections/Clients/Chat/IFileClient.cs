using System.IO;
using System.Threading.Tasks;

namespace SlackConnector.Connections.Clients.Chat
{
    interface IFileClient
    {
        Task PostFile(string slackKey, string channel, string filePath);
        Task PostFile(string slackKey, string channel, Stream stream, string fileName);
    }
}
