#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Air.Domain.Fares.Services.Ryanair.Models;

internal class TripDate
{
    [JsonPropertyName("dateOut")]
    public DateTime DateOut { get; set; }

    [JsonPropertyName("flights")]
    public List<Flight> Flights { get; set; }
}
