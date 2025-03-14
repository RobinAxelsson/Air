using System.Net.NetworkInformation;
using Air.Domain;


internal class RyanairServiceGateway : IDisposable
{
    private HttpClient _httpClient;
    private bool _disposed;

    public RyanairServiceGateway(Func<HttpMessageHandler> httpMessageHandlerFactory, string baseUrl)
    {
        _httpClient = new HttpClient(new ValidateAbsoluteUriDelegatingHandler(httpMessageHandlerFactory(), baseUrl));
        _httpClient.BaseAddress = new Uri(baseUrl);
        EnsureRyanairConnectivity(_httpClient);
    }

    public async Task<AirFlightFareDto[]> GetFlightFares(FlightSpecDto tripSpec)
    {
        string endpointUrl = GetEndpointUrl(tripSpec.Origin.ToString(), tripSpec.Destination.ToString(), tripSpec.Date.ToString("yyyy-MM-dd"));

        var httpResponseMessage = await _httpClient.GetAsync(endpointUrl);

        await EnsureSuccess(httpResponseMessage);

        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        if(string.IsNullOrEmpty(content) || content == "null")
        {
            throw new RyanairServiceRequestException($"The response content was empty or null for the request '{_httpClient.BaseAddress + endpointUrl}'");
        }

        var airFlightDtos = AirFlightFareDtoParser.ParseHttpResponseContent(content, _httpClient.BaseAddress + endpointUrl);

        //Ryanair API returns more flights then the days we enter in the dateout property in the request
        return airFlightDtos.Where(x => DateOnly.FromDateTime(x.DepartureUtc) == tripSpec.Date && x.Origin == tripSpec.Origin.ToString() && x.Destination == tripSpec.Destination.ToString()).ToArray();
    }

    private static string GetEndpointUrl(string origin, string destination, string dateOut)
    {
        //Make sure that the URL does not start with a slash!
        return $"availability?" +
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

        throw new RyanairServiceRequestException($"The ryanair Service call failed with response:\n" + responseDto.JsonSerializePretty());
    }

    private static void EnsureRyanairConnectivity(HttpClient _httpClient)
    {
        try
        {
            var domain = _httpClient.BaseAddress!.Host;
            using var ping = new Ping();
            ping.Send(domain);
        }
        catch (PingException ex)
        {
            throw new RyanairServiceConnectionException($"Failed to ping base address '{_httpClient.BaseAddress}'", ex);
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Head, _httpClient.BaseAddress);
            _ = _httpClient.Send(request);
        }
        catch (Exception ex)
        {
            throw new RyanairServiceConnectionException($"Host reachable but failed to do a http or https request to address '{_httpClient.BaseAddress}'", ex);
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed && _httpClient != null)
        {
            var localHttpClient = _httpClient;
            localHttpClient.Dispose();
            _httpClient = null!;
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
