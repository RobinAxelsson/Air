using Air.Domain.Fares.Models;

namespace Air.Domain.Fares.DataLayer.Dtos
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
