using System.IO;
using System.Threading.Tasks;

namespace SlackLibrary.Connections.Clients.File
{
    public interface IFileClient
    {
        Task PostFile(string slackKey, string channel, string filePath);
        Task PostFile(string slackKey, string channel, Stream stream, string fileName);
    }
}
