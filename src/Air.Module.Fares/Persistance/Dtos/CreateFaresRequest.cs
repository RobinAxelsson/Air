using Air.Module.Fares.Entities;

namespace Air.Module.Fares.Persistance.Dtos
{
    public sealed partial class CreateFaresRequest
    {
        internal FlightFare[] CreateFaresDtos { get; } = [];

        public CreateFaresRequest(IEnumerable<FlightFare> users)
        {
            CreateFaresDtos = users.ToArray();
        }
    }
}
