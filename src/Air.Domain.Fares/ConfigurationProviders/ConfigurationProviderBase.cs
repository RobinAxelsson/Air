// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;

namespace Air.Domain;

internal abstract class ConfigurationProviderBase
{
    public SqlConnectionString GetDbConnectionString()
    {
        return SqlConnectionStringParser.Parse(RetrieveConfigurationSettingValueThrowIfMissing("DbConnectionString"));
    }

    public string GetRyanairServiceBaseUrl()
    {
        var ryanairBaseUrl = RetrieveConfigurationSettingValueThrowIfMissing("RyanairBaseUrl");

        if(!Uri.TryCreate(ryanairBaseUrl, UriKind.Absolute, out var uriResult) || !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
        {
            throw new ConfigurationSettingInvalidException($"RyanairBaseUrl '{ryanairBaseUrl}' is not a valid url");
        }

        if (!ryanairBaseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
        {
            throw new ConfigurationSettingInvalidException($"RyanairBaseUrl '{ryanairBaseUrl}' should end with a forward slash");
        }

        return ryanairBaseUrl;
    }

    private string RetrieveConfigurationSettingValueThrowIfMissing(string key)
    {
        var valueAsRetrieved = RetrieveConfigurationSettingValue(key);
        if (valueAsRetrieved == null)
        {
            throw new ConfigurationSettingMissingException($"The Configuration Setting with Key: {key}, is Missing from the Configuration file");
        }
        else if (valueAsRetrieved.Length == 0)
        {
            throw new ConfigurationSettingValueEmptyException($"The Configuration Setting with Key: {key}, Exists but its value is Empty");
        }
        else if (IsWhiteSpaces(valueAsRetrieved))
        {
            throw new ConfigurationSettingValueEmptyException($"The Configuration Setting with Key: {key}, Exists but its value is White spaces.");
        }

        return valueAsRetrieved;
    }

    private static bool IsWhiteSpaces(string valueAsRetrieved)
    {
        foreach (var chr in valueAsRetrieved)
        {
            if (!char.IsWhiteSpace(chr))
            {
                return false;
            }
        }

        return true;
    }

    protected abstract string? RetrieveConfigurationSettingValue(string key);
}
