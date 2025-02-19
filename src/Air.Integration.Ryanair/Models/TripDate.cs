#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Air.Integration.Ryanair.Models;

internal class TripDate
{
    [JsonPropertyName("dateOut")]
    public DateTime DateOut { get; set; }

    [JsonPropertyName("flights")]
    public List<Flight> Flights { get; set; }
}
