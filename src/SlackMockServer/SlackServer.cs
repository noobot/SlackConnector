using System;
using WireMock.Server;
using WireMock.Settings;

namespace SlackMockServer
{
	public class SlackServer : IDisposable
	{
		public FluentMockServer HttpServer { get; protected set; }

		public SlackServer(int port, int startTimeout = 30000, bool useSSL = false)
		{
			this.HttpServer = FluentMockServer.Start(new FluentMockServerSettings() { StartTimeout = startTimeout, Port = port, UseSSL = useSSL });
		}

		public void Dispose()
		{
			HttpServer.Dispose();
		}
	}
}
