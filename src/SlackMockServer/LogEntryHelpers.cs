using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WireMock.Logging;
using WireMock.Server;

namespace SlackMockServer
{
	public static class LogEntryHelpers
	{
		public static LogEntry Filter(this IEnumerable<LogEntry> entries, string path, Func<LogEntry, bool> predicate)
		{
			return entries.Where(_ => _.RequestMessage.Path == path)
				.Where(predicate)
				.FirstOrDefault();
		}

		public static LogEntry RetryFilter(this FluentMockServer server, string path, Func<LogEntry, bool> predicate)
		{
			var entry = Policy
				.HandleResult<LogEntry>(_ => _ == null)
				.WaitAndRetry(200, i => TimeSpan.FromMilliseconds(100))
				.Execute(() =>
				{
					try
					{
						var tmpEntry = server.LogEntries.Filter(path, predicate);
						return tmpEntry;
					}
					catch (InvalidOperationException) // Handle collection was modified... multithread issue due to library
					{
						return null;
					}
				});
			return entry;
		}
	}
}
