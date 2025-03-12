using Air.Domain;

internal sealed class ValidateAbsoluteUriDelegatingHandler : DelegatingHandler
{
    private readonly string _baseUrl;
    private bool _isValidated = false;

    public ValidateAbsoluteUriDelegatingHandler(HttpMessageHandler innerHandler, string baseUrl) : base(innerHandler)
    {
        _baseUrl = baseUrl;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!_isValidated)
        {
            if (!request.RequestUri!.AbsoluteUri.StartsWith(_baseUrl))
            {
                //The last part of the base url is lost when the path starts with a '/'
                throw new RyanairServiceRequestException($"The request URI '{request.RequestUri!.AbsoluteUri}' does not start with the base URL '{_baseUrl}'");
            }

            var subParts = request.RequestUri!.AbsoluteUri.Split('/').Where(x => x != string.Empty);
            var distinctParts = subParts.Distinct();
            if (subParts.Count() > distinctParts.Count())
            {
                var duplicated = subParts.Except(distinctParts);
                throw new RyanairServiceRequestException($"The request URI '{request.RequestUri!.AbsoluteUri}' has duplicated sub parts '{string.Join(", ", duplicated)}'");
            }

            _isValidated = true;
        }

        return base.SendAsync(request, cancellationToken);
    }
}
