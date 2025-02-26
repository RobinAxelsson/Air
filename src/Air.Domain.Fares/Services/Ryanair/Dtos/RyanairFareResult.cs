using Air.Domain.Fares.Services.Ryanair.ResourceModel;

namespace Air.Domain.Fares.Services.Ryanair.Dtos
{
    public class RyanairFareResult
    {
        public required RyanairFare[] Fares { get; init; }
        public RyanairFareResultStatus Status { get; init; }
        public string? ErrorMessage { get; set; }
    }

    public enum RyanairFareResultStatus
    {
        Unknown,
        Success,
        Failed
    }
}
