using Air.Module.Fares.Services.Email.Dtos;

namespace Air.Module.Fares.Services.Email;

internal static class EmailTemplateGenerator
{
    private const string EmailTemplate = @"
        @model FlightFareDto
        <html>
        <head>
            <style>
                body { font-family: Arial, sans-serif; }
                .container { max-width: 600px; margin: auto; padding: 20px; }
                .header { font-size: 24px; font-weight: bold; }
                .fare-info { margin-top: 20px; }
                .fare-info p { font-size: 18px; }
                .highlight { color: #d9534f; font-weight: bold; }
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>Cheapest Flight from Gothenburg to Lisbon</div>
                <div class='fare-info'>
                    @if (Model != null)
                    {
                        <p>Airline: <span class='highlight'>@Model.Airline</span></p>
                        <p>Price: <span class='highlight'>€@Model.Price</span></p>
                        <p>Departure: <span class='highlight'>@Model.DepartureDate.ToString(""yyyy-MM-dd"")</span></p>
                    }
                    else
                    {
                        <p>No flights available.</p>
                    }
                </div>
            </div>
        </body>
        </html>";

    public async Task<string> GenerateEmailTemplateAsync(List<SurfFaresDto> fares)
    {
        var cheapestFare = fares
            .Where(f => f.Origin == "GOT" && f.Destination == "LIS")
            .OrderBy(f => f.Price)
            .FirstOrDefault();

        var engine = new RazorLight.RazorLightEngineBuilder()
            .UseMemoryCachingProvider()
            .Build();

        return await engine.CompileRenderStringAsync("emailTemplate", EmailTemplate, cheapestFare);
    }
}
