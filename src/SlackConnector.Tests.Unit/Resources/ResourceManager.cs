using System.IO;
using System.Reflection;

namespace SlackConnector.Tests.Unit.Resources
{
    public static class ResourceManager
    {
        public static string GetHandShakeResponseJson()
        {
            return ReadResource("Responses.HandShake.json");
        }

        public static string GetAttachmentsJson()
        {
            return ReadResource("Inputs.Attachments.json");
        }

        private static string ReadResource(string path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "SlackConnector.Tests.Unit.Resources." + path;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}