// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Air.Domain;

internal class DataFacade
{
    private readonly SqlConnectionString _dbConnectionString;

    public DataFacade(SqlConnectionString dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
        SqlConnectionValidator.EnsureConnection(_dbConnectionString);
    }

    [return: NotNull]
    internal async Task<AirFlight[]> GetAirFlights(FlightSpecDto tripSpec)
    {
        using var dbContext = new AirDbContext(_dbConnectionString.ToString());

        var flights = await dbContext.AirFlights
            .Where(f => f.Origin == tripSpec.Origin && f.Destination == tripSpec.Destination && DateOnly.FromDateTime(f.DepartureUtc) == tripSpec.Date)
            .ToListAsync();

        if(flights == null)
        {
            throw new DbContextReturnNullException();
        }

        return flights.ToArray();
    }

    internal async Task<AirFlight[]> PersistAirFlights(AirFlight[] flightFares)
    {
        using var dbContext = new AirDbContext(_dbConnectionString.ToString());

        //Get the flight fares for the
        await dbContext.AirFlights.AddRangeAsync(flightFares);

        await dbContext.SaveChangesAsync();

        return flightFares;
    }
}
