using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Air.Domain
{
    public static class TypeExtensions
    {
        public static string JsonSerializerSerializePretty<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true });
        }
    }
}
