namespace Air.Domain;

internal static class HttpResponseParser
{
    public static HttpResponseDto Parse(HttpResponseMessage response, string? responseBody = null)
    {
        return new HttpResponseDto
        {
            StatusCode = (int)response.StatusCode,
            ReasonPhrase = response.ReasonPhrase,
            IsSuccessStatusCode = response.IsSuccessStatusCode,
            Headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value)),
            ResponseBody = responseBody,
            RequestUri = response.RequestMessage?.RequestUri?.ToString(),
            Method = response.RequestMessage?.Method.Method
        };
    }
}
