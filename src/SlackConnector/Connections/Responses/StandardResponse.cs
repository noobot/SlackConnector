namespace SlackConnector.Connections.Responses
{
    internal class StandardResponse
    {
        public bool Ok { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// timestamp
        /// </summary>
        public string Ts { get; set; }
        public string Channel { get; set; }

        public string Needed { get; set; }

    }
}