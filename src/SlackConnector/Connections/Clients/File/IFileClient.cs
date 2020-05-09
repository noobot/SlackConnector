using System.IO;
using System.Threading.Tasks;

using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.File
{
    internal interface IFileClient
    {
        Task PostFile(string slackKey, string channel, string filePath);
        Task PostFile(string slackKey, string channel, Stream stream, string fileName);
        Task DownloadFile(string slackKey, SlackFile file, string path);
    }
}
