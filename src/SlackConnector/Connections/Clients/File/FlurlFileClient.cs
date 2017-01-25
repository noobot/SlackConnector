using System.IO;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.File
{
    internal class FlurlFileClient : IFileClient
    {
        private readonly IResponseVerifier _responseVerifier;
        internal const string FILE_UPLOAD_PATH = "/api/files.upload";

        public FlurlFileClient(IResponseVerifier responseVerifier)
        {
            _responseVerifier = responseVerifier;
        }

        public async Task PostFile(string slackKey, string channel, string filePath)
        {
            var response = await ClientConstants
                       .HANDSHAKE_PATH
                       .AppendPathSegment(FILE_UPLOAD_PATH)
                       .SetQueryParam("token", slackKey)
                       .SetQueryParam("channels", channel)
                       .SetQueryParam("filename", Path.GetFileName(filePath))
                       .GetJsonAsync<StandardResponse>();

            _responseVerifier.VerifyResponse(response);
        }

        public Task PostFile(string slackKey, string channel, Stream stream, string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}