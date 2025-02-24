namespace Air.Module.Fares;

public sealed class FareModule
{
    public async Task CreateWeekendSurfReport(int weekNumber, int year) => throw new NotImplementedException();
    public async Task<object> GetWeekendSurfReport(int weekNumber, int year) => throw new NotImplementedException();

    public async Task UpdateSurfFares(int weekNumber, int year)
    {
        //Uses ryan air client to get the fares to surf destinations for the weekend from the given weekNumber and year
        //Get dates
        //Get destinations - first only GOT and Lisbon
        //Get origin and destination
        throw new NotImplementedException();
    }
}
