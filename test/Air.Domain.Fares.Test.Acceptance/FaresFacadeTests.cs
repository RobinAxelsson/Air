// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Air.Domain.Fares.AcceptanceTests.TestDoubles;
using Air.Domain.Fares.Tests.AcceptanceTests.TestMediators;

namespace Air.Domain.Fares.Tests.AcceptanceTests
{
    public class FaresFacadeTests
    {
        private static (TestMediator mediator, FaresFacade faresFacade) CreateFaresFacade()
        {
            var testMediator = new TestMediator();
            return (testMediator, new FaresFacade(new ServiceLocatorForAcceptanceTesting(testMediator)));
        }

        [Category("AcceptanceTests")]
        [Test]
        public async Task SyncFares_WhenCalledWithGotToStn_xFaresUpdatedOrCreatedInDb()
        {
            var (mediator, faresFacade) = CreateFaresFacade();

            try
            {
                mediator.EnableRealServiceEndpoint = true;
                var syncFlightFaresResult = await faresFacade.SyncFlightFares(new FlightSpecDto() {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                    Origin = AirportCode.GOT,
                    Destination = AirportCode.STN,
                });

                await Assert.That(syncFlightFaresResult.FlightsCreated > 0 || syncFlightFaresResult.FlightsUpdated > 0).IsTrue();
                var faresJson = JsonSerializer.Serialize(syncFlightFaresResult, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine(faresJson);
            }
            finally
            {
                faresFacade.Dispose();
            }
        }
    }
}
