// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Air.Domain;

namespace Air.Domain;

internal static class AirportCodeValidator
{
    private static HashSet<AirportCode> _airportCodes = new HashSet<AirportCode>(Enum.GetValues<AirportCode>()[1..]);
    private static string AirportCodes() => String.Join(", ", _airportCodes);
    public static void EnsureValid(string airportCode)
    {
        var errors = ValidateProperties(airportCode);
        if (errors != null)
        {
            throw new InvalidAirportException(errors);
        }
    }

    public static string? ValidateWithErrorResult(string airportCode)
    {
        return ValidateProperties(airportCode);
    }

    public static string? ValidateWithErrorResult(AirportCode airportCode)
    {
        return ValidateProperties(airportCode.ToString());
    }

    private static string? ValidateProperties(string airportCode)
    {
        var errors = new StringBuilder();

        if (string.IsNullOrWhiteSpace(airportCode))
        {
            errors.AppendLine($"AirportCode was null or empty string. Valid codes are: {AirportCodes()}");
        }

        if (!AirportCodes().Contains(airportCode))
        {
            errors.AppendLine($"AirportCode code '{airportCode}' is not valid. Valid codes are: {AirportCodes()}");
        }

        return errors.Length != 0 ? errors.ToString() : null;
    }
}
