// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain.Fares.Test.Shared.TestDataGenerators.Helpers;

namespace Air.Domain.Fares.Test.Shared.TestDataGenerators;

public static class FlightSpecDtoFactory
{
    private static bool s_cloneIsValidated;

    /// <summary>
    /// The properties for the original get set with default values except the ones specified in the input action
    /// </summary>
    /// <param name="customize"></param>
    /// <returns></returns>
    public static FlightSpecDto Customizable(Action<FlightSpecDtoClone> customize)
    {
        EnsureCloneIsValidated();

        var clone = new FlightSpecDtoClone();
        customize(clone);
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
        if (s_cloneIsValidated)
        {
            return;
        }

        TripSpecDtoCloneValidator.EnsureCloneIsIdentical();
        s_cloneIsValidated = true;
    }
}
