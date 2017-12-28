namespace SlackConnector.Models
{
    public class SlackUser
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Image { get; internal set; }
        public string WhatIDo { get; internal set; }
        public bool Deleted { get; internal set; }
        public long TimeZoneOffset { get; internal set; }
        public bool? Online { get; internal set; }
        public bool IsBot { get; internal set; }
        public bool IsGuest { get; internal set; }
        public string StatusText { get; internal set; }
        public bool IsAdmin { get; internal set; }

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
    }
}