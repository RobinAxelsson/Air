// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

internal static class AirportParser
{
    private static HashSet<Airport> _airportSet = new HashSet<Airport>(Enum.GetValues<Airport>());
    private readonly static string _airportCodes = String.Join(", ", Enum.GetNames(typeof(Airport)));
    public static Airport ParseAirportCode(string airportCode)
    {
        if (string.IsNullOrWhiteSpace(airportCode))
        {
            throw new InvalidAirportException("Airport was null or empty string");
        }

        airportCode = airportCode.Trim().ToUpper();

        if (!Enum.TryParse(airportCode, out Airport result))
        {
            throw new InvalidAirportException($"Airport code '{airportCode}' is not valid. Valid codes are: {_airportCodes}");
        }

        return result;
    }

    public static string? ValidationMessage(Airport airport)
    {
        return !_airportSet.Contains(airport) ? $"The Airport: {airport} is not a valid Airport. Valid values are: {_airportCodes}" : null;
    }
}
