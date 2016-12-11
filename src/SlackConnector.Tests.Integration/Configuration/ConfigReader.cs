using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SlackConnector.Tests.Integration.Configuration
{
    public class ConfigReader : IConfigReader
    {
        private Config Current { get; set; }

        public Config GetConfig()
        {
            if (Current == null)
            {
                string fileName = Path.Combine(Environment.CurrentDirectory, "configuration", "config.json");
                if (!File.Exists(fileName))
                {
                    Assert.Inconclusive("Unable to load config file from: " + fileName);
                }

                string json = File.ReadAllText(fileName);
                if (string.IsNullOrEmpty(json))
                {
                    Assert.Inconclusive("Unable to load config");
                }

                Current = JsonConvert.DeserializeObject<Config>(json);
            }

            if (string.IsNullOrEmpty(Current?.Slack?.ApiToken))
            {
                Assert.Inconclusive("Slack API is missing");
            }

            return Current;
        }
    }
}