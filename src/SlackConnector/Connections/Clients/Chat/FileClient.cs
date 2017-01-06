using System;
using System.IO;
using System.Threading.Tasks;
using RestSharp;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.Chat
{
    class FileClient : IFileClient
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
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            request.AddParameter("token", slackKey);
            request.AddParameter("channels", channel);
            request.AddParameter("filename", fileName);
            request.AddFile("file", data, fileName);
            await _requestExecutor.Execute<StandardResponse>(request);
        }
    }
}
