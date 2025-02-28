using System.Text.Json;
using Air.Domain.Fares.Services.Ryanair.Dtos;
using Air.Domain.Fares.Services.Ryanair.Helpers;
using Air.Domain.Fares.Services.Ryanair.Models;

namespace Air.Domain.Fares.Services.Ryanair;

internal class RyanairClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RyanairClient(Func<HttpMessageHandler> httpMessageHandlerFactory)
    {
        _httpClient = new HttpClient(httpMessageHandlerFactory());
        _httpClient.BaseAddress = new Uri("https://www.ryanair.com/");
    }

    private static string GetRequestUrl(string origin, string destination, string dateOut)
    {
        return $"api/booking/v4/sv-se/availability?" +
            $"ADT=2&TEEN=0&CHD=0&INF=0&Origin={origin}&Destination={destination}&promoCode=" +
            $"&IncludeConnectingFlights=false&DateOut={dateOut}&DateIn=" +
            $"&FlexDaysBeforeOut=2&FlexDaysOut=2&FlexDaysBeforeIn=2&FlexDaysIn=2" +
            $"&RoundTrip=false&IncludePrimeFares=false&ToUs=AGREED";
    }

    public async Task<RyanairFareResult> GetRyanairFare(string origin, string destination, string date)
    {
        string requestUrl = GetRequestUrl("GOT", "STN", "2025-04-22");

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string
            string content = await response.Content.ReadAsStringAsync();

            var flightSearchResponse = JsonSerializer.Deserialize<FlightSearchResponse>(content, s_jsonSerializerOptions);

            if (flightSearchResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize response");
            }

            return ResponseMapper.MapFlightSearchResponse(flightSearchResponse);
        }
        else
        {
            throw new Exception($"Request failed with status code {response.StatusCode}");
        }
    }
}
