using System;
using System.Text.Json;

namespace Air.Domain
{
    public static class TypeExtensions
    {
        public static string JsonSerializerSerializeWriteIndented<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
