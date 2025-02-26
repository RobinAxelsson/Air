#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Air.Domain.Fares.Services.Ryanair.Models;

internal class Segment
{
    [JsonPropertyName("segmentNr")]
    public int SegmentNr { get; set; }

    [JsonPropertyName("origin")]
    public string Origin { get; set; }

    [JsonPropertyName("destination")]
    public string Destination { get; set; }

    [JsonPropertyName("flightNumber")]
    public string FlightNumber { get; set; }

    [JsonPropertyName("time")]
    public List<DateTime> Time { get; set; }

    [JsonPropertyName("timeUTC")]
    public List<DateTime> TimeUTC { get; set; }

    [JsonPropertyName("duration")]
    public string Duration { get; set; }
}
