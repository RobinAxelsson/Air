// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Air.Domain.Fares.AcceptanceTests.TestDoubles;
using Air.Domain.Fares.Tests.AcceptanceTests.TestMediators;
using Xunit.Abstractions;

namespace Air.Domain.Fares.Tests.AcceptanceTests
{
    public class FaresFacadeTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FaresFacadeTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Trait("", "AcceptanceTest")]
        [Fact]
        public async Task SyncFares_WhenCalled_xFaresUpdatedInDb()
        {
            var faresFacade = new FaresFacade(new ServiceLocatorForAcceptanceTesting(new TestMediator()));

            var syncFlightFaresResult = await faresFacade.SyncFlightFares(new FlightSpecDto() {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                Origin = AirportCode.GOT,
                Destination = AirportCode.STN,
            });

            Assert.True(syncFlightFaresResult.FlightsCreated > 0 || syncFlightFaresResult.FlightsUpdated > 0);
            var faresJson = JsonSerializer.Serialize(syncFlightFaresResult, new JsonSerializerOptions { WriteIndented = true });
            _testOutputHelper.WriteLine(faresJson);
        }
    }
}
