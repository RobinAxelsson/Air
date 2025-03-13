// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

internal static class AirFlightsIdentifyer
{
    internal static (AirFlight[] airFlightsToUpdate, AirFlight[] airFlightsToCreate) IdentifyUpdateAndCreate(AirFlight[] oldFlights, AirFlightFareDto[] newFlightFares)
    {
        var matchedFlightsToUpdate = new List<(AirFlight oldFlight, AirFlightFareDto newFlight)>();
        var toBeCreatedDtos = new List<AirFlightFareDto>();

        foreach (var newFlight in newFlightFares)
        {
            var oldFlightMatchings = oldFlights.Where(f =>
                f.FlightNumber == newFlight.FlightNumber &&
                f.Origin == newFlight.Origin &&
                f.Destination == newFlight.Destination &&
                f.DepartureUtc.Date == newFlight.DepartureUtc.Date
                ).ToArray();

            if (oldFlightMatchings.Length == 0)
            {
                toBeCreatedDtos.Add(newFlight);
            }

            if (oldFlightMatchings.Length > 1)
            {
                throw new InvalidFlightMatchException("It was more then one flight found in the same direction with similar arrival and departure, should not be possible", new { oldFlightMatchings });
            }

            if (oldFlightMatchings.Length == 1)
            {
                matchedFlightsToUpdate.Add((oldFlightMatchings[0], newFlight));
            }
        }

        foreach (var (oldFlight, newFlight) in matchedFlightsToUpdate)
        {
            var newFare = new AirFare { Currency = newFlight.Currency, Fare = newFlight.Fare, Source = newFlight.SourceUrl };

            var newButDuplicatedFare = oldFlight.Fares.FirstOrDefault(f => f.Currency == newFare.Currency && f.Fare == newFare.Fare && f.Source == newFare.Source);
            if (newButDuplicatedFare != null)
            {
                newButDuplicatedFare.LastObservedUtc = DateTime.UtcNow;
                break;
            }

            oldFlight.Fares.Add(newFare);
        }

        var airFlightsToCreate = toBeCreatedDtos.Select(MapToAirFlight).ToArray();
        var airFlightsToUpdate = matchedFlightsToUpdate.Select(m => m.oldFlight).ToArray();

        return (airFlightsToUpdate, airFlightsToCreate);
    }

    private static AirFlight MapToAirFlight(AirFlightFareDto flightFare)
    {
        return new AirFlight
        {
            Airline = flightFare.Airline,
            ArrivalUtc = flightFare.ArrivalUtc,
            DepartureUtc = flightFare.DepartureUtc,
            Destination = flightFare.Destination,
            Fares = new List<AirFare> { new AirFare { Currency = flightFare.Currency, Fare = flightFare.Fare, Source = flightFare.SourceUrl } },
            FlightNumber = flightFare.FlightNumber,
            Origin = flightFare.Origin
        };
    }
}
