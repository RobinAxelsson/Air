// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain.Fares.Test.Acceptance.TestMediators
{
    internal sealed class TestMediator
    {
        public IEnumerable<AirFlightFareDto>? RyanairResponseForSyncFlightFare { get; set; }

        public ExceptionInformation? ExceptionInformation { get; set; }
        public bool UseDefaultRequest { get; internal set; }
    }

    internal sealed class ExceptionInformation
    {
        public ExceptionReason? ExceptionReason { get; set; }
    }

    internal enum ExceptionReason
    {
        NotFond = 404,
        BadRequest = 400,
        ProxyAuthenticationRequired = 407,
        BadGateway = 502,
        ServiceUnavailable = 503,
    }
}
