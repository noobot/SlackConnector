using System.IO;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Clients.File
{
    internal class FlurlFileClient : IFileClient
    {
        private readonly IResponseVerifier _responseVerifier;
        internal const string FILE_UPLOAD_PATH = "/api/files.upload";
        internal const string POST_FILE_VARIABLE_NAME = "file";

        public FlurlFileClient(IResponseVerifier responseVerifier)
        {
            _responseVerifier = responseVerifier;
        }

        public async Task PostFile(string slackKey, string channel, string filePath)
        {
            var httpResponse = await ClientConstants
                       .SlackApiHost
                       .AppendPathSegment(FILE_UPLOAD_PATH)
                       .SetQueryParam("token", slackKey)
                       .SetQueryParam("channels", channel)
                       .PostMultipartAsync(content => content.AddFile(POST_FILE_VARIABLE_NAME, filePath));

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<StandardResponse>(responseContent);
            _responseVerifier.VerifyResponse(response);
        }

        public async Task PostFile(string slackKey, string channel, Stream stream, string fileName)
        {
            var httpResponse = await ClientConstants
                       .SlackApiHost
                       .AppendPathSegment(FILE_UPLOAD_PATH)
                       .SetQueryParam("token", slackKey)
                       .SetQueryParam("channels", channel)
                       .PostMultipartAsync(content => content.AddFile(POST_FILE_VARIABLE_NAME, stream, fileName));

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<StandardResponse>(responseContent);
            _responseVerifier.VerifyResponse(response);
        }
    }
}