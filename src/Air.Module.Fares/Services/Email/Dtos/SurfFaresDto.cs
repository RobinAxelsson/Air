namespace Air.Module.Fares.Services.Email.Dtos;
internal sealed class SurfFaresDto
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public decimal Price { get; set; }
    public string Airline { get; set; }
    public DateTime DepartureDate { get; set; }
}
