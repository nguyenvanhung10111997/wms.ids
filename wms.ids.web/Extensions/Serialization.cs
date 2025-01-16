

using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace wms.ids.web.Extensions
{
    public static class Serialization
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            }));
        }
        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null || !byteArray.Any())
                return default;

            return JsonSerializer.Deserialize<T>(byteArray, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            });
        }

    }
}

