// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

//We use the same file for easier maintenance and easier comparison
namespace Air.Domain;

[ExcludeFromCodeCoverage]
public sealed class FlightDurationComparisonException : AirFaresBusinessBaseException
{
    public override string Reason => "Flight duration difference";

    public FlightDurationComparisonException(string message, object properties) : base(message += Environment.NewLine + properties.JsonSerializerSerializePretty())
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class InvalidAirportException : AirFaresBusinessBaseException
{
    public override string Reason => "Invalid airport code";
    public InvalidAirportException()
    {
    }

    public InvalidAirportException(string message)
        : base(message)
    {
    }

    public InvalidAirportException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class InvalidCurrencyException : AirFaresBusinessBaseException
{
    public InvalidCurrencyException()
    {
    }

    public InvalidCurrencyException(string message)
        : base(message)
    {
    }

    public override string Reason => "Invalid airport code";
}

[ExcludeFromCodeCoverage]
public sealed class InvalidFlightMatchException : AirFaresBusinessBaseException
{
    public InvalidFlightMatchException(string message, object properties) : base(message + Environment.NewLine + properties.JsonSerializerSerializePretty())
    {
    }

    public override string Reason => "More flight numbers";
}

[ExcludeFromCodeCoverage]
public sealed class InvalidFlightFareException : AirFaresBusinessBaseException
{
    public InvalidFlightFareException()
    {
    }

    public InvalidFlightFareException(string message)
        : base(message)
    {
    }

    public InvalidFlightFareException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public override string Reason => "Invalid flight fare";
}

[ExcludeFromCodeCoverage]
public sealed class InvalidTripSpecException : AirFaresBusinessBaseException
{
    public InvalidTripSpecException()
    {
    }

    public InvalidTripSpecException(string message)
        : base(message)
    {
    }

    public InvalidTripSpecException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public override string Reason => "Invalid trip spec";
}

[ExcludeFromCodeCoverage]
public sealed class FlightFareNotFollowingSpecificationException : AirFaresBusinessBaseException
{
    public FlightFareNotFollowingSpecificationException(string message)
        : base(message)
    {
    }

    public override string Reason => "Flights not following trip spec";
}
