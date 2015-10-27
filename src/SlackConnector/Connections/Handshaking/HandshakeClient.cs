using System.Threading.Tasks;
using SlackConnector.Connections.Handshaking.Models;

namespace SlackConnector.Connections.Handshaking
{
    internal class HandshakeClient : IHandshakeClient
    {
        public Task<SlackHandshake> FirmShake(string slackKey)
        {
            throw new System.NotImplementedException();
        }
    }
}