using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace SlackConnector.Tests.Integration.Configuration
{
    public class ConfigReader : IConfigReader
    {
        private Config Current { get; set; }

        public Config GetConfig()
        {
            if (Current == null)
            {
                string fileName = Path.Combine(GetAssemblyDirectory(), "configuration", "config.json");
                if (!File.Exists(fileName))
                {
                    throw new InvalidConfiguration("Unable to load config file from: " + fileName);
                }

                string json = File.ReadAllText(fileName);
                if (string.IsNullOrEmpty(json))
                {
                    throw new InvalidConfiguration("Unable to load config");
                }

                Current = JsonConvert.DeserializeObject<Config>(json);
            }

            if (string.IsNullOrEmpty(Current?.Slack?.ApiToken))
            {
                throw new InvalidConfiguration("Slack API is missing");
            }

            return Current;
        }

        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }

    public class InvalidConfiguration : Exception
    {
        public InvalidConfiguration(string message) : base(message)
        { }
    }
}