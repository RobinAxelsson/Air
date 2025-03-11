using System.Text.Json;

namespace Air.Domain;
internal static class AirFlightFareDtoParser
{
    private static readonly JsonSerializerOptions jsonSerializerOptionsForFlightFareDeserialization = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static AirFlightFareDto[] ParseHttpResponseContent(string content, string baseUrl)
    {
        var availability = JsonSerializer.Deserialize<Availability>(content, jsonSerializerOptionsForFlightFareDeserialization);

        if (availability == null)
        {
            throw new RyanairServiceRequestException($"Failed to deserialize response: {content}");
        }

        var trip = availability.Trips[0];
        var origin = trip.Origin;
        var destination = trip.Destination;
        var currency = availability.Currency;
        var dates = trip.Dates;

        var fares = new List<AirFlightFareDto>();
        foreach (var date in dates)
        {
            foreach (var flight in date.Flights)
            {
                var fare = new AirFlightFareDto()
                {
                    Origin = AirportParser.ParseAirportCode(origin),
                    Destination = AirportParser.ParseAirportCode(destination),
                    Currency = CurrencyParser.ParseCurrencyCode(currency),
                    Fare = flight.RegularFare.Fares.First().Amount,
                    FlightNumber = flight.FlightNumber,
                    DepartureUtc = flight.TimeUTC[0],
                    ArrivalUtc = flight.TimeUTC[1],
                    SourceUrl = baseUrl,
                    Airline = "Ryanair",
                };

                fares.Add(fare);
            }
        }

        return [.. fares];
    }
}
