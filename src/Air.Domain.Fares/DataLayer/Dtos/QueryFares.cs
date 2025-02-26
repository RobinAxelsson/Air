using Air.Domain.Fares.Enums;

namespace Air.Domain.Fares.DataLayer.Dtos
{
    public record QueryFares
    {
        public required DateTime Date { get; set; }
        public required Airport Origin { get; set; }
        public required Airport Destination { get; set; }
    }
}
