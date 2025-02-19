namespace Air.Module.Fares.Entities
{
    public sealed class FlightFare
    {
        public required int Id { get; init; }
        public string FareId { get; } = Guid.NewGuid().ToString();
        public required string Airline { get; init; }
        public required string Currency { get; init; }
        public required string Origin { get; init; }
        public required string Destination { get; init; }
        public required decimal Amount { get; init; }
        public required decimal PublishedFare { get; init; }
        public required string FlightNumber { get; init; }
        public required string CustomAirlineID { get; init; }
        public required DateTime Departure { get; init; }
        public required DateTime DepartureUtc { get; init; }
        public required DateTime Arrival { get; init; }
        public required DateTime ArrivalUtc { get; init; }
        public required DateTime CollectedUtc { get; init; }
        public required int? FaresLeft { get; init; }
    }
}
