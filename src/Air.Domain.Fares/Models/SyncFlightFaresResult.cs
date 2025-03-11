// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

public sealed record SyncFlightFaresResult
{
    public int FlightsUpdated { get; init; }
    public int FlightsCreated { get; init; }
}
