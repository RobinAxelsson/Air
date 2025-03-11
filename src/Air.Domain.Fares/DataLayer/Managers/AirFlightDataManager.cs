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
        List<AirFlight> flights = null!;
        try
        {
             flights = await dbContext.AirFlights
             .Where(f => f.Origin == flightSpecDto.Origin.ToString() && f.Destination == flightSpecDto.Destination.ToString() && DateOnly.FromDateTime(f.DepartureUtc) == flightSpecDto.Date)
             .Include(f => f.Fares)
             .ToListAsync();

            if (flights == null)
            {
                throw new DbContextReturnNullException($"Dbcontext returned null with following {nameof(flightSpecDto)}", flightSpecDto);
            }
        }
        catch(Exception ex)
        {
            throw new DbContextGetAirFlightsException("Failed to get air flights, see inner exception", flightSpecDto, ex);
        }
        finally
        {
            dbContext.Dispose();
        }
     

        return flights.ToArray();
    }

    internal async Task CreateAirFlights(AirFlight[] airFlights)
    {
        var dbContext = new AirDbContext(_dbConnectionString.ToString());
        try
        {
            await dbContext.AirFlights.AddRangeAsync(airFlights);
            await dbContext.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw new DbContextCreateAirFlightsException("Failed to create air flights, see inner exception", airFlights, ex);
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    internal async Task UpdateAirFlights(AirFlight[] airFlights)
    {
        using var dbContext = new AirDbContext(_dbConnectionString.ToString());
        try
        {
            dbContext.AirFlights.UpdateRange(airFlights);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new DbContextUpdateAirFlightsException("Failed to update air flights, see inner exception", airFlights, ex);
        }
        finally
        {
            dbContext.Dispose();
        }
    }
}
