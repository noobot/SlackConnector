using System.Threading.Tasks;

namespace SlackConnector.Connections.Handshaking
{
    internal interface IHandshakeClient
    {
        /// <summary>
        /// No one likes a limp shake - AMIRITE?
        /// </summary>
        Task<SlackHandshake> FirmShake(string slackKey);
    }
}