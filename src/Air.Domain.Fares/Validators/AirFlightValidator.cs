// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

internal static class AirFlightValidator
{
    internal static void EnsureValid(AirFlight[] airFlights)
    {
        // TODO: add validation
        // Flight duration
        // Valid flight number
    }

    private static void EnsureSimilarFlightDuration(AirFlight oldFlight, AirFlightFareDto newFlightFare)
    {
        const double maxAllowedFlightDurationDifferenceInMinutes = 30;
        var oldFlightDuration = oldFlight.ArrivalUtc - oldFlight.DepartureUtc;
        var newFlightDuration = newFlightFare.ArrivalUtc - newFlightFare.DepartureUtc;
        var durationDifference = Math.Abs(oldFlightDuration.TotalMinutes - newFlightDuration.TotalMinutes);

        if (durationDifference > maxAllowedFlightDurationDifferenceInMinutes)
        {
            throw new FlightDurationComparisonException($"New and old flight duration have to large diff", new { OldDurationMin = oldFlightDuration.TotalMinutes, NewDurationMinutes = newFlightDuration.TotalMinutes, OldFlight = oldFlight, NewFlightFare = newFlightFare });
        }
    }
}
