// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain
{
    public sealed record FlightSpecDto
    {
        public required Airport Origin { get; init; }
        public required Airport Destination { get; init; }
        public required DateOnly Date { get; init; }
    }
}
