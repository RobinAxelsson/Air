// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain;

[ExcludeFromCodeCoverage]
public sealed class InvalidAirportException : AirFaresBusinessBaseException
{
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

    public override string Reason => "Invalid airport code";
}
