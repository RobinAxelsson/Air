#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Air.Domain;

internal class FlightSearchResponse
{
    [JsonPropertyName("termsOfUse")]
    public string TermsOfUse { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("currPrecision")]
    public int CurrPrecision { get; set; }

    [JsonPropertyName("routeGroup")]
    public string RouteGroup { get; set; }

    [JsonPropertyName("tripType")]
    public string TripType { get; set; }

    [JsonPropertyName("upgradeType")]
    public string UpgradeType { get; set; }

    [JsonPropertyName("trips")]
    public List<Trip> Trips { get; set; }

    [JsonPropertyName("serverTimeUTC")]
    public DateTime ServerTimeUTC { get; set; }
}
