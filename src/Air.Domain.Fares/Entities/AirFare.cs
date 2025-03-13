using Air.Domain;

namespace Air.Domain;

public sealed class AirFare
{
    public int FlightFareId { get; set; }
    public required Currency Currency { get; init; }
    public required decimal Fare { get; init; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastObservedUtc { get; set; } = DateTime.UtcNow;
    public required string Source { get; init; }
}
