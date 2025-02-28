using Air.Domain.Fares.Services.Ryanair.ResourceModel;

namespace Air.Domain.Fares.Services.Ryanair.Dtos
{
    public record RyanairFareResult
    {
        public required RyanairFare[] Fares { get; init; }
    }
}
