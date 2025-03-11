namespace Air.Domain;

internal static class TripSpecValidator
{
    private static readonly string _n = Environment.NewLine;
    public static void EnsureValid(FlightSpecDto tripSpec)
    {
        var errors = ValidateProperties(tripSpec);
        EnsureNoErrors(errors);
    }

    private static string? ValidateProperties(FlightSpecDto tripSpec)
    {
        var originErrorMessage = ValidateAirport(tripSpec.Origin);
        var destinationErrorMessage = ValidateAirport(tripSpec.Destination);
        var yearOfTravelErrorMessage = ValidateTravelYear(tripSpec.Date);

        var errorMessages = originErrorMessage + destinationErrorMessage + yearOfTravelErrorMessage;

        return errorMessages.Length == 0 ? null : errorMessages;
    }
    private static void EnsureNoErrors(string? errorMessages)
    {
        if (errorMessages != null)
        {
            throw new InvalidTripSpecException(errorMessages);
        }
    }

    private static string? ValidateTravelYear(DateOnly date)
    {
        const int minimumYear = 1914; // The year of the first commercial flight

        bool WithinTimeSpan(DateOnly date) => date.Year >= minimumYear && date.Year <= (DateTime.Today.Year + 10);

        return WithinTimeSpan(date) ? null
            : $"Flight must be between year {minimumYear} and {DateTime.Today.Year + 10} (inclusive)" + _n;
    }

    private static string? ValidateAirport(Airport airport)
    {
        var errorMessage = AirportParser.ValidationMessage(airport);
        if (errorMessage != null)
        {
            errorMessage += _n;
        }

        return errorMessage;
    }
}
