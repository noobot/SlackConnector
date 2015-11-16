using System.Threading.Tasks;
using SlackConnector.Connections.Responses;

namespace SlackConnector.Connections.Handshaking
{
    internal interface IHandshakeClient
    {
        /// <summary>
        /// No one likes a limp shake - AMIRITE?
        /// </summary>
        Task<HandshakeResponse> FirmShake(string slackKey);
    }
}