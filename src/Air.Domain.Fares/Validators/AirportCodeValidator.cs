// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

namespace Air.Domain;

internal static class AirportCodeValidator
{
    private readonly static string[] _airportCodes = Enum.GetNames(typeof(AirportCode));
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

    private static string? ValidateProperties(string airportCode)
    {
        var errors = new StringBuilder();

        if (string.IsNullOrWhiteSpace(airportCode))
        {
            errors.AppendLine("AirportCode was null or empty string");
        }

        airportCode = airportCode.Trim().ToUpper();

        if (!Enum.TryParse(airportCode, out AirportCode result))
        {
            errors.AppendLine($"AirportCode code '{airportCode}' is not valid. Valid codes are: {_airportCodes.JsonSerializerSerializeWriteIndentedUnsafeRelaxedJsonEscaping}");
        }

        return errors.Length != 0 ? errors.ToString() : null;
    }
}
