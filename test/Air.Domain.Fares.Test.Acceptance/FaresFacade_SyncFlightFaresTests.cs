// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Air.Domain.Fares.Test.Acceptance.Helpers;
using Air.Domain.Fares.Test.Acceptance.TestDoubles;
using Air.Domain.Fares.Test.Acceptance.TestMediators;

namespace Air.Domain.Fares.Test.Acceptance;
public class FaresFacade_SyncFlightFaresTests
{
    private static (FaresFacade faresFacade, TestMediator mediator) CreateDomainFacade()
    {
        var testMediator = new TestMediator();
        return (new FaresFacade(new ServiceLocatorForAcceptanceTesting(testMediator)), testMediator);
    }

    [Category("Acceptance Test")]
    [Test]
    public async Task SyncFlightFares_WhenCalledWithGotToStn_xFaresUpdatedOrCreatedInDb()
    {
        var (faresFacade, _) = CreateDomainFacade();

        try
        {
            var syncFlightFaresResult = await faresFacade.SyncFlightFares(FlightSpecDtoFactory.ValidStub());

            await Assert.That(syncFlightFaresResult.FlightsCreated > 0 || syncFlightFaresResult.FlightsUpdated > 0).IsTrue();
            var faresJson = JsonSerializer.Serialize(syncFlightFaresResult, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(faresJson);
        }
        finally
        {
            faresFacade.Dispose();
        }
    }

    [Test]
    [Category("Acceptance Test")]
    public async Task SyncFlightFares_WhenCalledWithAnInvalidAirport_ShouldThrow()
    {
        // Arrange
        var (domainFacade, _) = CreateDomainFacade();
        var invalidAirport = (AirportCode)int.MaxValue;
        var flightSpecDto = FlightSpecDtoFactory.Customizable(flightSpec => flightSpec.Origin = invalidAirport);

        try
        {
            // Act
            await domainFacade.SyncFlightFares(flightSpecDto);
            Assert.Fail($"We were expecting an {nameof(InvalidTripSpecException)} expection to be thrown but no exception was thrown");
        }
        catch (InvalidTripSpecException e)
        {
            await Assert.That(e.Message).Contains($"AirportCode code '{invalidAirport}' is not valid");
        }
        finally
        {
            domainFacade.Dispose();
        }
    }
}
