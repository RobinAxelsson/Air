using Air.Module.Fares.Enum;

namespace Air.Module.Fares.Persistance.Dtos
{
    public record QueryFares
    {
        public required DateTime Date { get; set; }
        public required Airport Origin { get; set; }
        public required Airport Destination { get; set; }
    }
}
