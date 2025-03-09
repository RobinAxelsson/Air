//using Air.Domain;

//internal class Program
//{
//    private static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);

//        // Add services to the container.
//        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//        builder.Services.AddOpenApi();

//        var app = builder.Build();
//        app.MapOpenApi();
//        app.UseHttpsRedirection();

//        EnableDefaultWeatherForecastEndpoint(app);

//        EnableRyanairServiceEndpoint(app);

//        app.Run();
//    }

//    private static void EnableRyanairServiceEndpoint(WebApplication app)
//    {
//        var flightSearchResponse = JsonSerializer.Serialize(File.ReadAllText("flightSearchResponse.json"));
//    }

//    private static void EnableDefaultWeatherForecastEndpoint(WebApplication app) //Use as health check
//    {
//        var summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        app.MapGet("/api/weatherforecast", () =>
//        {
//            var forecast = Enumerable.Range(1, 5).Select(index =>
//                new WeatherForecast
//                (
//                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                    Random.Shared.Next(-20, 55),
//                    summaries[Random.Shared.Next(summaries.Length)]
//                ))
//                .ToArray();
//            return forecast;
//        })
//        .WithName("GetWeatherForecast");
//    }
//}

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
