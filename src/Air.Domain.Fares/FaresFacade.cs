namespace Air.Domain;
public sealed class FaresFacade
{
    private AirFlightManager FlightFareManager { get; }

    public FaresFacade() : this(new ServiceLocator()) { }

    internal FaresFacade(ServiceLocatorBase serviceLocator)
    {
        FlightFareManager = new AirFlightManager(serviceLocator);
    }

    public async Task<SyncFlightFaresResult> SyncFlightFares(FlightSpecDto tripSpec)
    {
        return await FlightFareManager.SyncFlightFares(tripSpec);
    }
}
