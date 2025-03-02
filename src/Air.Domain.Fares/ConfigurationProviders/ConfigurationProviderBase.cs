// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;

internal abstract class ConfigurationProviderBase
{
    //public string GetImdbServiceBaseUrl()
    //{
    //    var imdbServiceBaseUrl = RetrieveConfigurationSettingValueThrowIfMissing("ImdbServiceBaseUrl");
    //    return imdbServiceBaseUrl.EndsWith("/", System.StringComparison.OrdinalIgnoreCase) ? imdbServiceBaseUrl : imdbServiceBaseUrl + "/";
    //}

    public string GetDbConnectionString()
    {
        return RetrieveConfigurationSettingValueThrowIfMissing("DbConnectionString");
    }

    private string RetrieveConfigurationSettingValueThrowIfMissing(string key)
    {
        var valueAsRetrieved = RetrieveConfigurationSettingValue(key);
        if (valueAsRetrieved == null)
        {
            throw new ConfigurationSettingException($"The Configuration Setting with Key: {key}, is Missing from the Configuration file");
        }
        else if (valueAsRetrieved.Length == 0)
        {
            throw new ConfigurationSettingException($"The Configuration Setting with Key: {key}, Exists but its value is Empty");
        }
        else if (IsWhiteSpaces(valueAsRetrieved))
        {
            throw new ConfigurationSettingException($"The Configuration Setting with Key: {key}, Exists but its value is White spaces.");
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
