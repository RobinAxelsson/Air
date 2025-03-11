namespace Air.Domain;

public sealed class AirFlight
{
    public int Id { get; set; }
    public required string Airline { get; init; }
    public required string Origin { get; init; }
    public required string Destination { get; init; }
    public required List<AirFare> Fares { get; init; }
    public required string FlightNumber { get; init; }
    public required DateTime DepartureUtc { get; init; }
    public required DateTime ArrivalUtc { get; init; }
    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
}
