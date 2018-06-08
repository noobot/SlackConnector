using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SlackConnector.EventAPI
{
	public class EventTypeConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object result = null;
			if (objectType.GetTypeInfo().IsEnum && reader.Value != null)
			{
				result = Activator.CreateInstance(objectType);

				try
				{
					var stringValue = reader.Value.ToString().Replace("_dot_", ".");
					result = Enum.Parse(objectType, stringValue, true);
				}
				catch (ArgumentException)
				{ }
			}

			return result;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.GetTypeInfo().IsEnum;
		}
	}
}
