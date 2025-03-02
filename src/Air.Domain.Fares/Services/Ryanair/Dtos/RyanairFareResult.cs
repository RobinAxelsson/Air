using Air.Domain;

public record RyanairFareResult
{
    public required RyanairFare[] Fares { get; init; }
}
