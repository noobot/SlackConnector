using System.Collections.Generic;
using System.Threading.Tasks;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;

namespace SlackConnector.Connections.Clients.Chat
{
    internal interface IChatClient
    {
        Task<PostMessageResponse> PostMessage(string slackKey, string channel, string text, IList<SlackAttachment> attachments);
	    Task<DeleteMessageResponse> DeleteMessage(string slackKey, string channel, string timeStamp);
    }
}