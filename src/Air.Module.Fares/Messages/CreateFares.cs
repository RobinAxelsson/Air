using Air.Module.Fares.Entities;

namespace Air.Module.Fares.Messages
{
    public sealed partial class CreateFares : MessageBase
    {
        internal FlightFare[] CreateFaresDtos { get; } = [];

        public CreateFares(IEnumerable<FlightFare> users)
        {
            CreateFaresDtos = users.ToArray();
        }
    }
}
