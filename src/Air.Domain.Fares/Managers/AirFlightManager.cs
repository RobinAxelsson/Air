// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Air.Domain;

internal class AirFlightManager : IDisposable
{
    private readonly ServiceLocatorBase _serviceLocator;
    private RyanairServiceGateway? _ryanairServiceGateway;
    private bool _disposed;

    private RyanairServiceGateway RyanairGateway => _ryanairServiceGateway ??= _serviceLocator.CreateRyanairGateway();

    private ConfigurationProviderBase ConfigurationProvider { get; }
    private DataFacade DataFacade { get; }
    public AirFlightManager(ServiceLocatorBase serviceLocator)
    {
        _serviceLocator = serviceLocator;
        ConfigurationProvider = serviceLocator.CreateConfigurationProvider();
        DataFacade = new DataFacade(ConfigurationProvider.GetDbConnectionString());
    }

    public async Task<SyncFlightFaresResult> SyncFlightFares(FlightSpecDto tripSpec)
    {
        TripSpecValidator.EnsureValid(tripSpec);
        var newFlightFares = await RyanairGateway.GetFlightFares(tripSpec);
        AirFlightFareDtoValidator.EnsureValid(newFlightFares);
        TripSpecAirFlightValidator.EnsureValid(newFlightFares, tripSpec);
        var oldAirFlights = await DataFacade.GetAirFlights(tripSpec);
        var (airFlightsToUpdate, airFlightsToCreate) = AirFlightsIdentifyer.IdentifyUpdateAndCreate(oldAirFlights, newFlightFares);
        await DataFacade.UpdateAirFlights(airFlightsToUpdate);
        await DataFacade.CreateAirFlights(airFlightsToCreate);
        return new SyncFlightFaresResult { FlightsUpdated = airFlightsToUpdate.Length, FlightsCreated = airFlightsToCreate.Length };
    }

    public async Task<ImmutableList<AirFlight>> GetAirFlights()
    {
        return await DataFacade.GetAirFlights();
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed && _ryanairServiceGateway != null)
        {
            var localImdbServiceGateway = _ryanairServiceGateway;
            localImdbServiceGateway.Dispose();
            _ryanairServiceGateway = null;
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
