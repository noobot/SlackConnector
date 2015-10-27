using System.Threading.Tasks;
using SlackConnector.Connections.Handshaking.Models;

namespace SlackConnector.Connections.Handshaking
{
    internal interface IHandshaker
    {
        /// <summary>
        /// No one likes a limp shake - AMIRITE?
        /// </summary>
        Task<SlackHandshake> FirmShake(string slackKey);
    }
}