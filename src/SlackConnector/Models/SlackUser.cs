namespace SlackConnector.Models
{
    public class SlackUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string WhatIDo { get; set; }
        public bool Deleted { get; set; }
        public long TimeZoneOffset { get; set; }
        public bool? Online { get; set; }
        public bool IsBot { get; set; }
        public bool IsGuest { get; set; }
        public string StatusText { get; set; }
        public bool IsAdmin { get; set; }

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