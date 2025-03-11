using System.Text.Json;

namespace Air.Domain;
internal static class AirFlightFareDtoParser
{
    private static readonly JsonSerializerOptions jsonSerializerOptionsForFlightFareDeserialization = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static AirFlightFareDto[] ParseHttpResponseContent(string content, string sourceUrl)
    {
        var availability = JsonSerializer.Deserialize<Availability>(content, jsonSerializerOptionsForFlightFareDeserialization);

        if (availability == null)
        {
            throw new RyanairServiceRequestException($"Failed to deserialize http content:", content);
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
                AirportCodeValidator.EnsureValid(origin);
                AirportCodeValidator.EnsureValid(destination);
                var fare = new AirFlightFareDto()
                {
                    Origin = origin,
                    Destination = destination,
                    Currency = CurrencyParser.ParseCurrencyCode(currency),
                    Fare = flight.RegularFare.Fares.First().Amount,
                    FlightNumber = flight.FlightNumber,
                    DepartureUtc = flight.TimeUTC[0],
                    ArrivalUtc = flight.TimeUTC[1],
                    SourceUrl = sourceUrl,
                    Airline = "Ryanair",
                }; 

                fares.Add(fare);
            }
        }

        return [.. fares];
    }
}
