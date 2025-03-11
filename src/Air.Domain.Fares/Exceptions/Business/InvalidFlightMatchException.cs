// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Air.Domain;

[ExcludeFromCodeCoverage]
public sealed class InvalidFlightMatchException : AirFaresBusinessBaseException
{
    public InvalidFlightMatchException(string message, object properties): base(message += Environment.NewLine + JsonSerializer.Serialize(properties, new JsonSerializerOptions { WriteIndented = true }))
    {
    }

    public override string Reason => "More flight numbers";
}
