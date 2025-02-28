using Air.Domain.Fares.DataLayer;
using Air.Domain.Fares.Models;
using Air.Domain.Fares.Services.Ryanair;
using Air.Domain.Fares.Services.Ryanair.ResourceModel;

namespace Air.Domain.Fares;

public sealed class FaresFacade
{
    private readonly Func<HttpMessageHandler> _httpMessageHandlerFactory;

    public FaresFacade() : this(HttpMessageHandlerFactory) { }

    internal FaresFacade(Func<HttpMessageHandler> httpMessageHandlerFactory)
    {
        _httpMessageHandlerFactory = httpMessageHandlerFactory;
    }

    public async Task SyncSurfFares()
    {
        var ryanairClient = new RyanairClient(_httpMessageHandlerFactory);

        var result = await ryanairClient.GetRyanairFare("STN", "LIS", "2025-03-05");

        var flightFares = MapToFlightFare(result.Fares);

        await PersistFlightFares(flightFares);
    }

    private static HttpMessageHandler HttpMessageHandlerFactory()
    {
        return new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15),
        };
    }

    private async Task PersistFlightFares(IEnumerable<FlightFare> flightFares)
    {
        using var dbContext = new AirDbContext();
        await dbContext.FlightFares.AddRangeAsync(flightFares);
        await dbContext.SaveChangesAsync();
    }

    private IEnumerable<FlightFare> MapToFlightFare(RyanairFare[] fares)
    {
        return fares.Select(x => new FlightFare
        {
            Arrival = x.Arrival,
            Currency = x.Currency,
            Departure = x.Departure,
            Destination = x.Destination,
            Fare = x.Amount,
            FlightNumber = x.FlightNumber,
            Origin = x.Origin
        });
    }
}
