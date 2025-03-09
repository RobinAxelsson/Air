using System.Text.Encodings.Web;
using System.Text.Json;
using Air.Domain;

internal class RyanairGateway
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions jsonSerializerOptionsForHttpResponseSerialization = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    public RyanairGateway(Func<HttpMessageHandler> httpMessageHandlerFactory, string baseUrl)
    {
        _httpClient = new HttpClient(httpMessageHandlerFactory());
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<FlightFareEntity[]> GetFlightFares(TripSpec tripSpec)
    {
        string requestUrl = GetRequestUrl(tripSpec.Origin.ToString(), tripSpec.Destination.ToString(), tripSpec.Date.ToString("yyyy-MM-dd"));

        var httpResponseMessage = await _httpClient.GetAsync(requestUrl);

        await EnsureSuccess(httpResponseMessage);

        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        return RyanairFlightFareParser.ParseHttpContent(content);
    }

    private static string GetRequestUrl(string origin, string destination, string dateOut)
    {
        //Make sure that the URL does not start with a slash!
        return $"api/booking/v4/sv-se/availability?" +
            $"ADT=2&TEEN=0&CHD=0&INF=0&Origin={origin}&Destination={destination}&promoCode=" +
            $"&IncludeConnectingFlights=false&DateOut={dateOut}&DateIn=" +
            $"&FlexDaysBeforeOut=2&FlexDaysOut=2&FlexDaysBeforeIn=2&FlexDaysIn=2" +
            $"&RoundTrip=false&IncludePrimeFares=false&ToUs=AGREED";
    }


    private static async Task EnsureSuccess(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync();
        var responseDto = HttpResponseParser.Parse(response, body);
        var responseJson = JsonSerializer.Serialize(responseDto, jsonSerializerOptionsForHttpResponseSerialization);

        throw new RyanairServiceRequestException($"The ryanair Service call failed with response: {responseJson}");
    }
}
