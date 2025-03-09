// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain;

[ExcludeFromCodeCoverage]
public sealed class AirSqlConnectionException : AirFaresTechnicalBaseException
{
    public override string Reason => "Unable to connect to sql server";

    public AirSqlConnectionException()
    {
    }

    public AirSqlConnectionException(string message)
        : base(message)
    {
    }

    public AirSqlConnectionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
