using System;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Ecommerce.Core
{
    public class JsonSerializer<T> : ISerializer<T>, IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull) return default(T);

            var str = Encoding.UTF8.GetString(data.ToArray());

            return JsonConvert.DeserializeObject<T>(str);
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            return data == null ? null : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}