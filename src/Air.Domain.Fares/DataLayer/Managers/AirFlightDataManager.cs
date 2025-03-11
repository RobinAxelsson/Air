// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;

namespace Air.Domain;

internal class AirFlightDataManager
{
    private readonly SqlConnectionString _dbConnectionString;

    public AirFlightDataManager(SqlConnectionString dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }

    internal async Task<AirFlight[]> GetAirFlights(FlightSpecDto flightSpecDto)
    {
        using var dbContext = new AirDbContext(_dbConnectionString.ToString());

        var flights = await dbContext.AirFlights
            .Where(f => f.Origin == flightSpecDto.Origin && f.Destination == flightSpecDto.Destination && DateOnly.FromDateTime(f.DepartureUtc) == flightSpecDto.Date)
            .ToListAsync();

        if(flights == null)
        {
            throw new DbContextReturnNullException();
        }

        return flights.ToArray();
    }

    internal async Task CreateAirFlights(AirFlight[] airFlights)
    {
        using var dbContext = new AirDbContext(_dbConnectionString.ToString());

        await dbContext.AirFlights.AddRangeAsync(airFlights);

        await dbContext.SaveChangesAsync();
    }

    internal async Task UpdateAirFlights(AirFlight[] airFlights)
    {
        using var dbContext = new AirDbContext(_dbConnectionString.ToString());

        dbContext.AirFlights.UpdateRange(airFlights);

        await dbContext.SaveChangesAsync();
    }
}
