namespace Air.Domain;

internal static class TripSpecAirFlightValidator
{
    private static string _n = Environment.NewLine;
    public static void EnsureValid(IEnumerable<AirFlightFareDto> flightFares, FlightSpecDto flightSpec)
    {
        var errorMessagesSb = StringBuilderCache.Acquire();
        string? errors = null;
        foreach (var flightFare in flightFares)
        {
            errors = ValidateSpecification(flightFare, flightSpec);
            if (errors != null)
            {
                errorMessagesSb.Append(errors);
            }
        }

        errors = StringBuilderCache.GetStringAndRelease(errorMessagesSb);

        if (errors.Length != 0)
        {
            throw new FlightFareNotFollowingSpecificationException(errors);
        }
    }

    private static string? ValidateSpecification(AirFlightFareDto flightFare, FlightSpecDto flightSpec)
    {
        var originErrorMessage = flightFare.Origin != flightSpec.Origin.ToString() ? $"The origin '{flightFare.Origin}' does not match the flight specification '{flightSpec.Origin}'{_n}" : null;

        var destinationErrorMessage = flightFare.Destination != flightSpec.Destination.ToString() ? $"The destination '{flightFare.Destination}' does not match the flight specification '{flightSpec.Destination}'{_n}" : null;

        var travelDateErrorMessage = DateOnly.FromDateTime(flightFare.DepartureUtc) != flightSpec.Date ? $"The travel date '{flightFare.DepartureUtc.Date}' does not match the flight specification '{flightSpec.Date}'{_n}" : null;

        var errorMessages = originErrorMessage + destinationErrorMessage + travelDateErrorMessage;

        return errorMessages.Length == 0 ? null : errorMessages;
    }
}
