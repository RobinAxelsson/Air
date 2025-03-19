// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

namespace Air.Domain;

internal static class ServiceHttpUrlValidator
{
    public static void EnsureValid(string httpUrl, string service)
    {
        var schemeError = ValidateAbsoluteUriScheme(httpUrl);
        var httpMissingError = ValidateHttpScheme(httpUrl);
        var forwardSlashError = ValidateForwardSlash(httpUrl);
        var errors = schemeError + httpMissingError + forwardSlashError;
        if (errors.Length != 0)
        {
            throw new InvalidHttpUrlException($"Service '{service}' url '{httpUrl}' is invalid\n" + errors);
        }
    }

    private static string? ValidateAbsoluteUriScheme(string httpUrl)
    {
        if(!Uri.TryCreate(httpUrl, UriKind.Absolute, out var uri))
        {
            return "- an absolute uri is required\n";
        }

        return null;
    }

    private static string? ValidateHttpScheme(string httpUrl)
    {
        if(!Uri.TryCreate(httpUrl, UriKind.Absolute, out Uri? uri) || !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            return "- http or https is required in the url\n";
        }

        return null;
    }

    private static string? ValidateForwardSlash(string httpUrl)
    {
        return !httpUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? "- url should end with a forward slash\n" : null;
    }
}
