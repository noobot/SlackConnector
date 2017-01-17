namespace SlackConnector.Models
{
    public class SlackUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string FormattedUserId
        {
            get
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    return "<@" + Id + ">";
                }
                return string.Empty;
            }
        }

        public long TimeZoneOffset { get; set; }

        public bool? Online { get; set; }

        public bool IsBot { get; set; }
    }
}