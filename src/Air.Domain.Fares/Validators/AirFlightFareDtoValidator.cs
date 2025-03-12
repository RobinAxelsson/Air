namespace Air.Domain;

internal static class AirFlightFareDtoValidator
{
    private static readonly string _n = Environment.NewLine;
    public static void EnsureValid(IEnumerable<AirFlightFareDto> flightFares)
    {
        EnsureFlightFaresAreNotNull(flightFares);
        string? errorMessages = ValidateProperties(flightFares);
        EnsureNoErrors(errorMessages);
    }

    private static string? ValidateProperties(IEnumerable<AirFlightFareDto> flightFares)
    {
        var errorMessagesSb = StringBuilderCache.Acquire();
        foreach (var flightFare in flightFares)
        {
            var errors = ValidateProperties(flightFare);
            if (errors != null)
            {
                errorMessagesSb.Append(errors);
            }
        }

        var errorMessages = StringBuilderCache.GetStringAndRelease(errorMessagesSb);

        return errorMessages.Length == 0 ? null : errorMessages;
    }

    private static string? ValidateProperties(AirFlightFareDto flightFare)
    {
        const int maxLengthFlightNumber = 7;

        var originErrorMessage = AirportCodeValidator.ValidateWithErrorResult(flightFare.Origin);
        var destinationErrorMessage = AirportCodeValidator.ValidateWithErrorResult(flightFare.Destination);
        var flightNumberErrorMessage = StringValidator.Validate(nameof(AirFlightFareDto.FlightNumber), flightFare.FlightNumber, maxLengthFlightNumber);

        var airlineErrorMessage = StringValidator.Validate(nameof(AirFlightFareDto.Airline), flightFare.Airline);
        var travelYearErrorMessage = ValidateTravelYear(flightFare.DepartureUtc, flightFare.ArrivalUtc);
        var travelOrderErrorMessage = ValidateTravelOrder(flightFare.DepartureUtc, flightFare.ArrivalUtc);
        var fareErrorMessage = ValidateFare(flightFare.Fare);

        var errorMessages = originErrorMessage + destinationErrorMessage + flightNumberErrorMessage + travelYearErrorMessage + travelOrderErrorMessage + fareErrorMessage;

        return errorMessages.Length == 0 ? null : errorMessages;
    }
    private static void EnsureNoErrors(string? errorMessages)
    {
        if (errorMessages != null)
        {
            throw new InvalidFlightFareException(errorMessages);
        }
    }

    private static void EnsureFlightFaresAreNotNull(IEnumerable<AirFlightFareDto> flightFares)
    {
        if (!flightFares.Any())
        {
            throw new InvalidFlightFareException("The flightFares Collection Must Contain One or More FlightFares and Can't be Empty.");
        }
    }

    private static string? ValidateTravelYear(DateTime departure, DateTime arrival)
    {
        const int minimumYear = 1914; // The year of the first commercial flight

        bool WithinTimeSpan(DateTime date) => date.Year >= minimumYear && date.Year <= (DateTime.Today.Year + 10);

        return WithinTimeSpan(departure) && WithinTimeSpan(arrival) ? null
            : $"Both arrival and departure, must be between year {minimumYear} and {DateTime.Today.Year + 10} (inclusive)" + _n;
    }

    private static string? ValidateTravelOrder(DateTime departure, DateTime arrival)
    {
        const int maxFlightTimeHours = 100; // The longest commercial flight is 20 hours, but why not 100?

        return departure < arrival && (arrival - departure) < TimeSpan.FromHours(maxFlightTimeHours) ? null
        : $"The arrival date must be after the departure date and the flight can't be longer than {maxFlightTimeHours} hours" + _n;
    }

    private static string? ValidateFare(decimal fare) => fare <= 0 ? "The fare must be greater than 0" + _n : null;
}
