// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

internal class DataFacade
{
    private readonly SqlConnectionString _dbConnectionString;
    private AirFlightDataManager? _airFlightDataManager;
    private AirFlightDataManager AirFlightDataManager => _airFlightDataManager ??= new AirFlightDataManager(_dbConnectionString);

    public DataFacade(SqlConnectionString dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
        SqlConnectionValidator.EnsureConnection(_dbConnectionString);
    }

    internal async Task<AirFlight[]> GetAirFlights(FlightSpecDto tripSpec)
    {
        return await AirFlightDataManager.GetAirFlights(tripSpec);
    }

    internal async Task CreateAirFlights(AirFlight[] airFlights)
    {
        await AirFlightDataManager.CreateAirFlights(airFlights);
    }

    internal async Task UpdateAirFlights(AirFlight[] airFlights)
    {
        await AirFlightDataManager.UpdateAirFlights(airFlights);
    }
}
