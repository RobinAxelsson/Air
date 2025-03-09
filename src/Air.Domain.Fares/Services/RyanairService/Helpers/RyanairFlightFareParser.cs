using System.Text.Json;

namespace Air.Domain;
internal static class RyanairFlightFareParser
{
    private static readonly JsonSerializerOptions jsonSerializerOptionsForFlightFareDeserialization = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static FlightFareEntity[] ParseHttpContent(string content)
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

        var fares = new List<FlightFareEntity>();
        foreach (var date in dates)
        {
            foreach (var flight in date.Flights)
            {
                var fare = new FlightFareEntity()
                {
                    Origin = AirportParser.ParseAirportCode(origin),
                    Destination = AirportParser.ParseAirportCode(destination),
                    Currency = CurrencyParser.ParseCurrencyCode(currency),
                    Fare = flight.RegularFare.Fares.First().Amount,
                    FlightNumber = flight.FlightNumber,
                    DepartureUtc = flight.TimeUTC[0],
                    ArrivalUtc = flight.TimeUTC[1],
                    Airline = "Ryanair",
                };

                fares.Add(fare);
            }
        }

        return [.. fares];
    }
}
