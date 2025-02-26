#pragma warning disable CS8618
using System.Text.Json.Serialization;

namespace Air.Domain.Fares.Services.Ryanair.Models;

internal class Fare
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("hasDiscount")]
    public bool HasDiscount { get; set; }

    [JsonPropertyName("publishedFare")]
    public decimal PublishedFare { get; set; }

    [JsonPropertyName("discountInPercent")]
    public int DiscountInPercent { get; set; }

    [JsonPropertyName("hasPromoDiscount")]
    public bool HasPromoDiscount { get; set; }

    [JsonPropertyName("discountAmount")]
    public decimal DiscountAmount { get; set; }

    [JsonPropertyName("hasBogof")]
    public bool HasBogof { get; set; }

    [JsonPropertyName("isPrime")]
    public bool IsPrime { get; set; }
}
