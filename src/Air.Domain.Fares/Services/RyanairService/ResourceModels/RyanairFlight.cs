#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Air.Domain;

internal class RyanairFlight
{
    [JsonPropertyName("faresLeft")]
    public int FaresLeft { get; set; }

    [JsonPropertyName("flightKey")]
    public string FlightKey { get; set; }

    [JsonPropertyName("infantsLeft")]
    public int InfantsLeft { get; set; }

    [JsonPropertyName("regularFare")]
    public RegularFare RegularFare { get; set; }

    [JsonPropertyName("operatedBy")]
    public string OperatedBy { get; set; }

    [JsonPropertyName("segments")]
    public List<Segment> Segments { get; set; }

    [JsonPropertyName("flightNumber")]
    public string FlightNumber { get; set; }

    [JsonPropertyName("time")]
    public List<DateTime> Time { get; set; }

    [JsonPropertyName("timeUTC")]
    public List<DateTime> TimeUTC { get; set; }

    [JsonPropertyName("duration")]
    public string Duration { get; set; }
}
