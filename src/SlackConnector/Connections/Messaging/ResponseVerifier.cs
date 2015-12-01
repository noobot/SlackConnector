using Newtonsoft.Json;
using RestSharp;

namespace SlackConnector.Connections.Messaging
{
    internal class ResponseVerifier : IResponseVerifier
    {
        public T VerifyResponse<T>(IRestResponse response) where T : class
        {
            return JsonConvert.DeserializeObject(response.Content, typeof (T)) as T;
        }
    }
}