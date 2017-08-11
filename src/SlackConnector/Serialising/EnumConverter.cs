using System;
using System.Reflection;
using Newtonsoft.Json;

namespace SlackConnector.Serialising
{
    internal class EnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object result = null;
            if (objectType.GetTypeInfo().IsEnum && reader.Value != null)
            {
                result = Activator.CreateInstance(objectType);
                
                try
                {
                    result = Enum.Parse(objectType, reader.Value.ToString(), true);
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