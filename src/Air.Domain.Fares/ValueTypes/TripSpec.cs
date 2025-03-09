// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain
{
    public sealed record TripSpec
    {
        public required Airport Origin { get; init; }
        public required Airport Destination { get; init; }
        public required DateTime Date { get; init; }
    }
}
