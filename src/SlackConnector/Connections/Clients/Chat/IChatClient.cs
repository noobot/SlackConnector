using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    internal interface IChatClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slackKey"></param>
        /// <param name="channel"></param>
        /// <param name="text"></param>
        /// <param name="attachments"></param>
        /// <returns>message ID/timestamp, for example: 123456789.9875</returns>
        Task<string> PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments);

        /// <summary>
        /// Updates previously posted message
        /// </summary>
        /// <param name="slackKey"></param>
        /// <param name="timeStamp">Required: timestamp of message to update, e.g. 123456789.9875</param>
        /// <param name="channel"></param>
        /// <param name="text"></param>
        /// <param name="attachments"></param>
        /// <returns>message ID/timestamp, for example: 123456789.9875</returns>
        Task<string> UpdateMessage(string slackKey, string timeStamp, string channel, string text, IList<SlackAttachment> attachments);
    }
}