using Air.Domain.Fares.Models.atomic;

namespace Air.Domain;

public sealed record AirFlightFareDto
{
    public required string Airline { get; init; }
    public required Currency Currency { get; init; }
    public required string Origin { get; init; }
    public required string Destination { get; init; }
    public required decimal Fare { get; init; }
    public required string FlightNumber { get; init; }
    public required DateTime DepartureUtc { get; init; }
    public required DateTime ArrivalUtc { get; init; }
    public required string SourceUrl { get; init; }
    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
}
