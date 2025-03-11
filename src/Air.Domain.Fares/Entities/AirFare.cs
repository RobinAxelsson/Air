using Air.Domain.Fares.Models.atomic;

namespace Air.Domain;

public sealed class AirFare
{
    public int FlightFareId { get; set; }
    public required Currency Currency { get; init; }
    public required decimal Fare { get; init; }
    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
    public required string Source { get; init; }
}
