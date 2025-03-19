// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain.Fares.Test.Shared.TestDataGenerators.Helpers;
#nullable disable

//The clone is validated with reflection to match the original exact
public sealed record FlightSpecDtoClone
{
    public AirportCode? Origin { get; set; }
    public AirportCode? Destination { get; set; }
    public DateOnly? Date { get; set; }
    public Currency? Currency { get; set; }
}
