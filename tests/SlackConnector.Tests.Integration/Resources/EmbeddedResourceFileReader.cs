using System;
using System.IO;
using System.Reflection;

namespace SlackConnector.Tests.Integration.Resources
{
    public static class EmbeddedResourceFileReader
    {
        public static string ReadEmbeddedFileAsText(string file)
        {
            using (var reader = new StreamReader(ReadEmbeddedFile(file)))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream ReadEmbeddedFile(string file)
        {
            string resourcePath = $"{typeof(EmbeddedResourceFileReader).Namespace}.{file}";

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                throw new InvalidOperationException($"Unable to find '{resourcePath}' as an embedded resource");
            }

            return stream;
        }
    }
}