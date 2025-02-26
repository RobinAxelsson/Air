using System.Text.Json;
using Air.Domain.Fares;

namespace Air.Interface.CLI;
internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        if (args[0] == "--help" || args[0] == "-h")
        {
            PrintHelpText();
            return 0;
        }

        if (args.Length < 3)
        {
            PrintHelpText();
            return 1;
        }

        if (!int.TryParse(args[1], out int week) || week < 1 || week > 53)
        {
            Console.WriteLine("Error: Invalid week number.");
            PrintHelpText();
            return 1;
        }

        if (!int.TryParse(args[1], out int year) || year <= 3000 || week >= 1900)
        {
            Console.WriteLine("Error: Invalid year number.");
            PrintHelpText();
            return 1;
        }

        var faresModule = new FaresFacade();

        string command = args[0];
        switch (command)
        {
            case "update-surf-fares":
                Console.WriteLine($"Updating weekly surf fares for week: {week}...");
                await faresModule.UpdateSurfFares(week, year);
                break;

            case "create-weekend-report":
                Console.WriteLine($"Creating weekend surf report for week: {week}...");
                await faresModule.CreateWeekendSurfReport(week, year);
                break;

            case "get-weekend-report":
                Console.WriteLine($"Getting weekend surf report for week {week}...");
                var weekendSurfReport = await faresModule.GetWeekendSurfReport(week, year);
                var json = JsonSerializer.Serialize(weekendSurfReport, new JsonSerializerOptions { WriteIndented = true });

                string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                string fileName = $"weekend-report-week-{week}-{timestamp}.json";
                await File.WriteAllTextAsync(fileName, json);

                Console.WriteLine($"Report saved to {fileName}");
                break;

            case "create-emails":
                await faresModule.CreateEmails(year, week);
                Console.WriteLine("Sending emails...");
                break;

            case "send-emails":
                Console.WriteLine("Sending emails...");
                await faresModule.SendEmails(year, week);
                break;

            default:
                PrintHelpText();
                return 1;
        }
        return 0;
    }

    private static void PrintHelpText()
    {
        Console.WriteLine("Missing arguments. Usage: <command> <week> <year>");
        Console.WriteLine("Commands:");
        Console.WriteLine("  update-surf-fares <week> <year>  - Updates weekly surf fares");
        Console.WriteLine("  create-weekend-report <week> <year>  - Creates a weekend surf report");
        Console.WriteLine("  get-weekend-report <week> <year>  - Retrieves and saves the weekend surf report");
        Console.WriteLine("  send-emails  - Sends notification emails (TODO: Implement)");
    }
}
