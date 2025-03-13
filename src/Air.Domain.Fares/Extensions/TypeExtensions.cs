using System;
using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Air.Domain
{
    public static class TypeExtensions
    {
        //Todo issue with serializing string[] with this method
        public static string JsonSerializerSerializePretty<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true });
        }
    }
}
