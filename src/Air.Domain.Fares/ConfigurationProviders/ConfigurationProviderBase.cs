// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace Air.Domain;

internal abstract class ConfigurationProviderBase
{
    public SqlConnectionString GetDbConnectionString()
    {
        return SqlConnectionStringParser.Parse(RetrieveConfigurationSettingValueThrowIfMissing("DbConnectionString"));
    }

    public string GetRyanairServiceBaseUrl()
    {
        return RetrieveConfigurationSettingValueThrowIfMissing("RyanairBaseUrl");
    }

    private string RetrieveConfigurationSettingValueThrowIfMissing(string key)
    {
        return RetrieveConfigurationSettingValueThrowIfMissingCore(key);
    }

    protected abstract string RetrieveConfigurationSettingValueThrowIfMissingCore(string key);
}
