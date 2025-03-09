// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain;

[ExcludeFromCodeCoverage]
public sealed class InvalidConnectionStringException : AirFaresTechnicalBaseException
{
    public override string Reason => "Invalid connedction string";

    public InvalidConnectionStringException()
    {
    }

    public InvalidConnectionStringException(string message)
        : base(message)
    {
    }

    public InvalidConnectionStringException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
