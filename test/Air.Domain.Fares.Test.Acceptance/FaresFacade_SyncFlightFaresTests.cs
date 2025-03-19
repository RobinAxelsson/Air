// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Air.Domain.Fares.Test.Shared.Asserters;
using Air.Domain.Fares.Test.Shared.TestDataGenerators;

namespace Air.Domain.Fares.Test.Acceptance;
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class FaresFacade_SyncFlightFaresTests
{
    [Category("Acceptance Test")]
    [Test]
    public async Task SyncFlightFares_WhenCalledWithGotToStn_xFaresUpdatedOrCreatedInDb()
    {
        using var faresFacade = new FaresFacade();

        var syncFlightFaresResult = await faresFacade.SyncFlightFares(FlightSpecDtoFactory.ValidStub());

        await Assert.That(syncFlightFaresResult.FlightsCreated > 0 || syncFlightFaresResult.FlightsUpdated > 0).IsTrue();
        var faresJson = JsonSerializer.Serialize(syncFlightFaresResult, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(faresJson);
    }

    [Test, DependsOn(nameof(SyncFlightFares_WhenCalledWithGotToStn_xFaresUpdatedOrCreatedInDb))] //Needs to be something in the database to pass the test
    [Category("Acceptance Test")]
    public async Task GetFlightFares_ShouldReturnX()
    {
        using var faresFacade = new FaresFacade();
        var invalidAirport = (AirportCode)int.MaxValue;
        var flightSpecDto = FlightSpecDtoFactory.Customizable(flightSpec => flightSpec.Origin = invalidAirport);

        var airFlights = await faresFacade.GetAirFlight();

        await Assert.That(airFlights).IsNotEmpty();
        await Assert.That(airFlights.SelectMany(x => x.Fares)).IsNotEmpty();
    }

    [Test]
    [Category("Acceptance Test")]
    public async Task SyncFlightFares_WhenCalledWithAnInvalidAirport_ShouldThrow()
    {
        var faresFacade = new FaresFacade();
        var invalidAirport = (AirportCode)int.MaxValue;
        var flightSpecDto = FlightSpecDtoFactory.Customizable(flightSpec => flightSpec.Origin = invalidAirport);

        try
        {
            await faresFacade.SyncFlightFares(flightSpecDto);
            Assert.Fail($"We were expecting an {nameof(InvalidTripSpecException)} expection to be thrown but no exception was thrown");
        }
        catch (InvalidTripSpecException e)
        {
            AssertEx.EnsureExceptionMessageContains(e, $"AirportCode code '{invalidAirport}' is not valid");
        }
        finally
        {
            faresFacade.Dispose();
        }
    }
}
