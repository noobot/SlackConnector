using System.Threading.Tasks;
using SlackLibrary.Connections.Responses;

namespace SlackLibrary.Connections.Clients.Handshake
{
    public interface IHandshakeClient
    {
        /// <summary>
        /// No one likes a limp shake - AMIRITE?
        /// </summary>
        Task<HandshakeResponse> FirmShake(string slackKey);
    }
}