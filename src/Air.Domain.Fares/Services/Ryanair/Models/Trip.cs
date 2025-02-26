#pragma warning disable CS8618

using System.Text.Json.Serialization;

namespace Air.Domain.Fares.Services.Ryanair.Models;

internal class Trip
{
    [JsonPropertyName("origin")]
    public string Origin { get; set; }

    [JsonPropertyName("originName")]
    public string OriginName { get; set; }

    [JsonPropertyName("destination")]
    public string Destination { get; set; }

    [JsonPropertyName("destinationName")]
    public string DestinationName { get; set; }

    [JsonPropertyName("routeGroup")]
    public string RouteGroup { get; set; }

    [JsonPropertyName("tripType")]
    public string TripType { get; set; }

    [JsonPropertyName("upgradeType")]
    public string UpgradeType { get; set; }

    [JsonPropertyName("dates")]
    public List<TripDate> Dates { get; set; }
}
