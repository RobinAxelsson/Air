namespace Air.Domain;
public sealed class FaresFacade
{
    private FlightFareManager FlightFareManager { get; }

    public FaresFacade() : this(new ServiceLocator()) { }

    internal FaresFacade(ServiceLocatorBase serviceLocator)
    {
        FlightFareManager = new FlightFareManager(serviceLocator);
    }

    public async Task<IEnumerable<FlightFareEntity>> SyncFlightFares(TripSpec tripSpec)
    {
        return await FlightFareManager.SyncFlightFares(tripSpec);
    }
}
