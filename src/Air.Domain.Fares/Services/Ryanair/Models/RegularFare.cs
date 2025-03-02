#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Air.Domain;

internal class RegularFare
{
    [JsonPropertyName("fareKey")]
    public string FareKey { get; set; }

    [JsonPropertyName("fares")]
    public List<Fare> Fares { get; set; }
}
