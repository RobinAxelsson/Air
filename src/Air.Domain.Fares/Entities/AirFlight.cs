namespace Air.Domain;

public sealed class AirFlight
{
    public required string Airline { get; init; }
    public required Airport Origin { get; init; }
    public required Airport Destination { get; init; }
    public required List<AirFare> Fares { get; init; }
    public required string FlightNumber { get; init; }
    public required DateTime DepartureUtc { get; init; }
    public required DateTime ArrivalUtc { get; init; }
    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
}
