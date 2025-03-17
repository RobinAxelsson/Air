using System.Text.Json;
using Air.Domain;

namespace Air.Interface.CLI;
internal class Program
{
    private const string Help =
        "Usage: exe <command>\n"
        + "Commands:\n"
        + "  sync-fares - updates flight fares\n"
        + "  get-fares  - get flight fares\n";

    internal static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            throw new ArgumentException($"No arguments provided\n" + Help);
        }
        if (args.Length > 1)
        {
            throw new ArgumentException($"Only one argument is allowed, arguments provided '{string.Join(", ", args)}'\n" + Help);
        }

        if (args[0] == "--help" || args[0] == "-h")
        {
            Console.WriteLine(Help);
            return 0;
        }

        var action = args[0];

        if(action == "sync-fares")
        {
            using var faresFacade = new FaresFacade();
         
            var flightFares = await faresFacade.SyncFlightFares(new FlightSpecDto() {
                Date = new DateOnly(2025, 03, 22),
                Origin = AirportCode.GOT,
                Destination = AirportCode.STN,
                Currency = Currency.SEK
            });

            Console.WriteLine(JsonSerializer.Serialize(flightFares, new JsonSerializerOptions { WriteIndented = true }));
            return 0;
        }


        if(action == "get-fares")
        {
            using var faresFacade = new FaresFacade();

            var airFlights = await faresFacade.GetAirFlight();

            Console.WriteLine(JsonSerializer.Serialize(airFlights, new JsonSerializerOptions { WriteIndented = true }));
            return 0;
        }

        throw new ArgumentException($"Invalid command: {args[0]}\n" + Help);
    }
}
