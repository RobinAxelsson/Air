using System.Text.Json;

namespace Air.Domain.Fares.Tests.ClassTests;

public class ResponseMapperTests
{
    [Fact]
    public void ParseFlightSearchResponse_fromFile_ok()
    {
        var json = File.ReadAllText("TestData/booking-response-3-flights.json");
        var flightSearchResponse = JsonSerializer.Deserialize<FlightSearchResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.NotNull(flightSearchResponse);

        var result = ResponseMapper.MapFlightSearchResponse(flightSearchResponse);

        Assert.Equal(3, result.Fares.Length);

        var fare0 = result.Fares[0];
        Assert.Equal((decimal)846.15, fare0.Amount);
        Assert.Equal((decimal)846.15, fare0.PublishedFare);
        Assert.Equal("SEK", fare0.Currency);
        Assert.Equal("GOT", fare0.Origin);
        Assert.Equal("STN", fare0.Destination);
        Assert.Equal("FR 965", fare0.FlightNumber);
        Assert.Equal("2025-04-10T10:45:00", fare0.Departure.ToString("s"));
        Assert.Equal("2025-04-10T11:40:00", fare0.Arrival.ToString("s"));
    }
}
