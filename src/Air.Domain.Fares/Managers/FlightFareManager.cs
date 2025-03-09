// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

internal class FlightFareManager
{
    private readonly ServiceLocatorBase _serviceLocator;
    private RyanairGateway? _ryanairGateway;
    private RyanairGateway RyanairGateway => _ryanairGateway ??= _serviceLocator.CreateRyanairGateway();

    private ConfigurationProviderBase ConfigurationProvider { get; }
    private DataFacade DataFacade { get; }
    public FlightFareManager(ServiceLocatorBase serviceLocator)
    {
        _serviceLocator = serviceLocator;
        ConfigurationProvider = serviceLocator.CreateConfigurationProvider();
        DataFacade = new DataFacade(ConfigurationProvider.GetDbConnectionString());
    }

    public async Task<IEnumerable<FlightFareEntity>> SyncFlightFares(TripSpec tripSpec)
    {
        TripSpecValidator.EnsureTripSpecIsValid(tripSpec);
        var flightFares = await RyanairGateway.GetFlightFares(tripSpec);
        FlightFareValidator.EnsureFlightFaresAreValid(flightFares);
        return await DataFacade.PersistFlightFares(flightFares);
    }
}
