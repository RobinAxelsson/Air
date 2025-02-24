using Air.Integration.Ryanair.ResourceModel;

namespace Air.Integration.Ryanair.Results
{
    public class RyanairFareResult
    {
        public required RyanairFare[] Fares { get; init; }
        public RyanairFareResultStatus Status { get; init;  }
        public string? ErrorMessage { get; set; }
    }

    public enum RyanairFareResultStatus
    {
        Unknown,
        Success,
        Failed
    }
}
