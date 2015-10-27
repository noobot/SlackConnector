using System.Threading.Tasks;
using SlackConnector.Connections.Handshaking.Models;

namespace SlackConnector.Connections.Handshaking
{
    internal class HandShaker : IHandshaker
    {
        public Task<SlackHandshake> FirmShake(string slackKey)
        {
            throw new System.NotImplementedException();
        }
    }
}