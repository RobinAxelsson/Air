// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http.Json;
using System.Net;
using Air.Domain.Fares.Test.Acceptance.TestMediators;

namespace Air.Domain.Fares.Test.Acceptance.TestDoubles;

internal sealed class HttpMessageHandlerSpy(TestMediator testMeditor) : DelegatingHandler
{
    private readonly TestMediator _testMediator = testMeditor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return base.SendAsync(request, cancellationToken);

        //if (_testMediator.ExceptionInformation == null)
        //{
        //    var flightFareDtos = _testMediator.RyanairResponseForSyncFlightFare!;
        //    return WriteFlightFareToResponse(request, flightFareDtos);
        //}

        //if (_testMediator.ExceptionInformation.ExceptionReason != null)
        //{
        //    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        StatusCode = (HttpStatusCode)_testMediator.ExceptionInformation.ExceptionReason,
        //        Content = new StringContent("Exception Occured")
        //    };

        //    return Task.FromResult(httpResponseMessage);
        //}
    }

    //private static Task<HttpResponseMessage> WriteFlightFareToResponse(HttpRequestMessage request, IEnumerable<AirFlightFareDto> flightFareDtos)
    //{
    //    RyanairAvailability availability = default!;

    //    var absolutePath = request.RequestUri!.AbsolutePath;

    //    if (absolutePath.Contains("/availability?"))
    //    {
    //        availability = MapAirFlightFareDtoToAvailability(flightFareDtos);
    //    }

    //    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
    //    {
    //        Content = JsonContent.Create(availability)
    //    };

    //    return Task.FromResult(httpResponseMessage);
    //}

    //private static RyanairAvailability MapAirFlightFareDtoToAvailability(IEnumerable<AirFlightFareDto> flightFareDtos) => throw new NotImplementedException();
}
