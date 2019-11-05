using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WireMock;

namespace SlackMockServer
{
	public static partial class SlackServerHelpers
	{
		public static string GenerateTimestamp()
		{
			var sTicks = DateTime.Now.Ticks.ToString();
			var ticks = sTicks.Substring(sTicks.Length - 10, 10);
			return ticks.Substring(0, 5) + "." + ticks.Substring(5, 5);
		}

		public static IEnumerable<string> GetParameterValuesFromPostOrGet(this RequestMessage request, string key)
		{
			IEnumerable<string> values = request.GetParameter(key);

			if (values is null || !values.Any())
			{
				if (request.Body is null)
					return Enumerable.Empty<string>();

				var qs = HttpUtility.ParseQueryString(request.Body.TrimStart('?'));
				values = qs[key]?.Split(',');
			}

			return values ?? Enumerable.Empty<string>();
		}

		public static string GetSafeParameter(this RequestMessage requestMessage, string key)
		{
			var uri = new Uri(requestMessage.Url);
			if (string.IsNullOrEmpty(uri.Query))
				return null;
			var query = QueryHelpers.ParseQuery(uri.Query);
			if (!query.ContainsKey(key))
				return null;
			return query[key];
		}

		public static string GetParameterValueFromPostOrGet(this RequestMessage request, string key)
		{
			var value = request.GetSafeParameter(key);

			if (value is null)
			{
				if (request.Body is null)
					return null;
				var qs = HttpUtility.ParseQueryString(request.Body.TrimStart('?'));
				return qs[key];
			}

			return value;
		}

		private static string GetUserIdFromChannel(this RequestMessage request)
		{
			var channel = request.GetChannel();
			if (channel.StartsWith("DM"))
				return channel.Substring(2);
			return null;
		}
	}
}
