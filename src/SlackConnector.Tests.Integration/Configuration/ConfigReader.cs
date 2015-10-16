using System;
using System.IO;
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
                string fileName = Path.Combine(Environment.CurrentDirectory, @"configuration\config.json");
                string json = File.ReadAllText(fileName);
                Current = JsonConvert.DeserializeObject<Config>(json);
            }

            return Current;
        }
    }
}