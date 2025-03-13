// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain.Fares.Test.Acceptance.Helpers;

internal static class FlightSpecDtoFactory
{
    private static bool _cloneIsValidated;

    //we do all this with the clone:
    //1. so that we can keep immutablilty in the model
    //2. harder to break the tests
    public static FlightSpecDto DefaultCustomizableCore(Func<FlightSpecDtoClone> func)
    {
        EnsureCloneIsValidated();
        var clone = func == null ? new FlightSpecDtoClone() : func();
        return new FlightSpecDto
        {
            Currency = clone.Currency ?? Currency.SEK,
            Date = clone.Date ?? DateOnly.FromDateTime(DateTime.Now).AddDays(7),
            //We use GOT STN becuae there are flights every day
            Origin = clone.Origin ?? AirportCode.GOT,
            Destination = clone.Destination ?? AirportCode.STN,
        };

    }

    

    public static FlightSpecDto ValidStub()
    {
        EnsureCloneIsValidated();

        return new FlightSpecDto
        {
            Currency = Currency.SEK,
            Date = DateOnly.FromDateTime(DateTime.Now).AddDays(7),
            //We use GOT STN becuae there are flights every day
            Origin = AirportCode.GOT,
            Destination = AirportCode.STN,
        };
    }

    private static void EnsureCloneIsValidated()
    {
        if (!_cloneIsValidated)
        {
            TripSpecDtoCloneValidator.EnsureCloneIsIdentical();
            _cloneIsValidated = true;
        }
    }
}
