using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Models
{
    class SlackUnknownReaction : ISlackReaction
    {
        public string RawData { get; set; }
        public SlackUser User { get; set; }
        public double Timestamp { get; set; }
        public string Reaction { get; set; }

        public SlackReactionType ReactionType { get { return SlackReactionType.unknown; } }
    }
}
