// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

internal class AirFlightManager
{
    private readonly ServiceLocatorBase _serviceLocator;
    private RyanairServiceGateway? _ryanairGateway;
    private RyanairServiceGateway RyanairGateway => _ryanairGateway ??= _serviceLocator.CreateRyanairGateway();

    private ConfigurationProviderBase ConfigurationProvider { get; }
    private DataFacade DataFacade { get; }
    public AirFlightManager(ServiceLocatorBase serviceLocator)
    {
        _serviceLocator = serviceLocator;
        ConfigurationProvider = serviceLocator.CreateConfigurationProvider();
        DataFacade = new DataFacade(ConfigurationProvider.GetDbConnectionString());
    }

    public async Task<AirFlight[]> SyncFlightFares(FlightSpecDto tripSpec)
    {
        TripSpecValidator.EnsureTripSpecIsValid(tripSpec);
        var flightFares = await RyanairGateway.GetFlightFares(tripSpec);
        AirFlightFareDtoValidator.EnsureValid(flightFares);
        var oldFlightFlights = await DataFacade.GetAirFlights(tripSpec);
        var (updatedAirFlights, flightFareDtosToCreate) = AirFlightsIdentifyer.IdentifyUpdateAndCreate(oldFlightFlights, flightFares);


        //Update or create new fares
        //Persist the fares
        //Create a result object
        //Return the result object

        throw new NotImplementedException();
        //return await DataFacade.PersistAirFlights(flightFares);
    }
}

internal static class AirFlightsIdentifyer
{
    internal static (AirFlight[] updatedAirFlights, AirFlightFareDto[] flightFareDtosToCreate) IdentifyUpdateAndCreate(AirFlight[] oldFlights, AirFlightFareDto[] newFlightFares)
    {
        var toBeUpdated = new List<(AirFlight oldFlight, AirFlightFareDto newFlight)>();
        var toBeCreated = new List<AirFlightFareDto>();

        void EnsureSimilarFlightDuration(AirFlight oldFlight, AirFlightFareDto newFlightFare)
        {
            const double maxAllowedFlightDurationDifferenceInMinutes = 30;
            var oldFlightDuration = oldFlight.ArrivalUtc - oldFlight.DepartureUtc;
            var newFlightDuration = newFlightFare.ArrivalUtc - newFlightFare.DepartureUtc;
            var durationDifference = Math.Abs(oldFlightDuration.TotalMinutes - newFlightDuration.TotalMinutes);

            if (durationDifference > maxAllowedFlightDurationDifferenceInMinutes)
            {
                throw new FlightDurationComparisonException($"New and old flight duration have to large diff", new { OldDurationMin = oldFlightDuration.TotalMinutes, NewDurationMinutes = newFlightDuration.TotalMinutes, OldFlight = oldFlight, NewFlightFare = newFlightFare });
            }
        }

        foreach (var newFlight in newFlightFares)
        {
            var oldFlightMatchings = oldFlights.Where(f =>
                f.FlightNumber == newFlight.FlightNumber &&
                f.Origin == newFlight.Origin &&
                f.Destination == newFlight.Destination &&
                f.DepartureUtc.Date == newFlight.DepartureUtc.Date
                ).ToArray();

            if (oldFlightMatchings.Length == 0)
            {
                toBeCreated.Add(newFlight);
            }

            if (oldFlightMatchings.Length > 1)
            {
                throw new InvalidFlightMatchException("It was more then one flight found in the same direction with similar arrival and departure", new { oldFlightMatchings });
            }

            if (oldFlightMatchings.Length == 1)
            {
                EnsureSimilarFlightDuration(oldFlightMatchings[0], newFlight);
                toBeUpdated.Add((oldFlightMatchings[0], newFlight));
            }
        }

        foreach (var (oldFlight, newFlight) in toBeUpdated)
        {
            var newFare = new AirFare { Currency = newFlight.Currency, Fare = newFlight.Fare, Source = newFlight.SourceUrl };
        }
    }
}
