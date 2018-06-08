using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlackConnector.Serialising
{
    internal class SecondEpochConverter : DateTimeConverterBase
	{
		private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue(String.Format("{0:0}", ((DateTime)value - _epoch).TotalSeconds));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null) { return null; }
			return _epoch.AddSeconds((long)reader.Value);
		}
	}
}
