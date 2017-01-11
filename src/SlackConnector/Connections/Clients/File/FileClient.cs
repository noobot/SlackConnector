using System.IO;
using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.File
{
    internal class FileClient : IFileClient
    {
        private readonly IRequestExecutor _requestExecutor;
        internal const string FILE_UPLOAD_PATH = "/api/files.upload";

        public FileClient(IRequestExecutor requestExecutor)
        {
            _requestExecutor = requestExecutor;
        }

        public async Task PostFile(string slackKey, string channel, string file)
        {
            var request = new RestRequest(FILE_UPLOAD_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("channels", channel);
            request.AddParameter("filename", Path.GetFileName(file));
            request.AddFile("file", file);

            await _requestExecutor.Execute<StandardResponse>(request);
        }

        public async Task PostFile(string slackKey, string channel, Stream stream, string fileName)
        {
            var request = new RestRequest(FILE_UPLOAD_PATH);
            request.AddParameter("token", slackKey);
            request.AddParameter("channels", channel);
            request.AddParameter("filename", fileName);

            byte[] data = await ReadByteArray(stream);
            request.AddFile("file", data, fileName);

            await _requestExecutor.Execute<StandardResponse>(request);
        }

        private async Task<byte[]> ReadByteArray(Stream stream)
        {
            var memoryStream = stream as MemoryStream;
            if (memoryStream != null)
            {
                return memoryStream.ToArray();
            }

            using (memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
