// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain;

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
