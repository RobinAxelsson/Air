namespace Air.Domain;

//public because of EF exposure
public sealed class FlightFare
{
    //[Obsolete("This constructor is for EF reflection", true)]
    //public FlightFare()
    //{

    //}
    public int Id { get; init; }
    //public string FareId { get; } = Guid.NewGuid().ToString();
    //public required string Airline { get; init; }
    public required string Currency { get; init; }
    public required string Origin { get; init; }
    public required string Destination { get; init; }
    public required decimal Fare { get; init; }
    //public required decimal PublishedFare { get; init; }
    public required string FlightNumber { get; init; }
    //public required string CustomAirlineID { get; init; }
    public required DateTime Departure { get; init; }
    //public required DateTime DepartureUtc { get; init; }
    public required DateTime Arrival { get; init; }
    //public required DateTime ArrivalUtc { get; init; }
    //public required DateTime CollectedUtc { get; init; }
    //public required int? FaresLeft { get; init; }
}
