using System;

namespace SlackConnector.Connections
{
    public class ProxySettings
    {
        public string Url { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public ProxySettings(string url, string username, string password)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            Url = url;
            Username = username;
            Password = password;
        }
    }
}