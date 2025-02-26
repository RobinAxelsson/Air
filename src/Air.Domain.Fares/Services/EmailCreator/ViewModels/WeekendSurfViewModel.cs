namespace Air.Domain.Fares.Services.EmailCreator.ViewModels;

//public because of Razorligt
public sealed class WeekendSurfViewModel
{
    public required string Origin { get; set; }
    public required string Destination { get; set; }
    public required decimal Price { get; set; }
    public required string Airline { get; set; }
    public required DateTime DepartureDate { get; set; }
}
