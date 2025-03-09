// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace Air.Domain;

internal class DataFacade
{
    private readonly SqlConnectionString _dbConnectionString;

    public DataFacade(SqlConnectionString dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
        SqlConnectionValidator.EnsureConnection(_dbConnectionString);
    }
    internal async Task<IEnumerable<FlightFareEntity>> PersistFlightFares(IEnumerable<FlightFareEntity> flightFares)
    {
        using var dbContext = new AirDbContext(_dbConnectionString.ToString());

        await dbContext.FlightFares.AddRangeAsync(flightFares);

        await dbContext.SaveChangesAsync();

        return flightFares;
    }
}
