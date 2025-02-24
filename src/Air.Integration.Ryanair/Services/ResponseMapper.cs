using System.Runtime.CompilerServices;
using Air.Integration.Ryanair.Models;
using Air.Integration.Ryanair.ResourceModel;
using Air.Integration.Ryanair.Results;

[assembly: InternalsVisibleTo("Air.Integration.Ryanair.Tests")]

namespace Air.Integration.Ryanair.Services
{
    internal static class ResponseMapper
    {
        public static RyanairFareResult MapFlightSearchResponse(FlightSearchResponse flightSearchResponse)
        {
            var trip = flightSearchResponse.Trips[0];
            var origin = trip.Origin;
            var destination = trip.Destination;
            var currency = flightSearchResponse.Currency;
            var dates = trip.Dates;

            var fares = new List<RyanairFare>();
            foreach (var date in dates)
            {
                foreach (var flight in date.Flights)
                {
                    var fare = new RyanairFare
                    {
                        Origin = origin,
                        Destination = destination,
                        Currency = currency,
                        Amount = flight.RegularFare.Fares.First().Amount,
                        PublishedFare = flight.RegularFare.Fares.First().PublishedFare,
                        FlightNumber = flight.FlightNumber,
                        Departure = flight.Time[0],
                        Arrival = flight.Time[1],
                        CollectedUtc = DateTime.UtcNow,
                        FaresLeft = flight.FaresLeft,
                        FareKey = flight.RegularFare.FareKey,
                        DepartureUtc = flight.TimeUTC[0],
                        ArrivalUtc = flight.TimeUTC[1]
                    };

                    fares.Add(fare);
                }
            }


            return new RyanairFareResult
            {
                Fares = fares.ToArray()
            };
        }
    }
}
