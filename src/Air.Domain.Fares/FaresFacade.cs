using System.Collections.Immutable;

namespace Air.Domain;
public sealed class FaresFacade : IDisposable
{
    private bool _disposed;

    private AirFlightManager AirFlightManager { get; }

    public FaresFacade() : this(new ServiceLocator()) { }

    internal FaresFacade(ServiceLocatorBase serviceLocator)
    {
        AirFlightManager = new AirFlightManager(serviceLocator);
    }

    public async Task<SyncFlightFaresResult> SyncFlightFares(FlightSpecDto tripSpec)
    {
        return await AirFlightManager.SyncFlightFares(tripSpec);
    }

    public async Task<ImmutableList<AirFlight>> GetAirFlight()
    {
        return await AirFlightManager.GetAirFlights();
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed && AirFlightManager != null)
        {
            AirFlightManager.Dispose();
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
