using Air.Domain.Fares.Enums;

namespace Air.Domain.Fares.Services.EmailCreator.Dtos;

internal sealed class WeekendSurfViewModelDto
{
    public required Airport Origin { get; set; }
    public required Airport Destination { get; set; }
    public required decimal Price { get; set; }
    public required Airline Airline { get; set; }
    public required DateTime DepartureDate { get; set; }
}
