// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Air.Domain;

internal sealed class ConfigurationProvider : ConfigurationProviderBase
{
    private readonly IConfigurationRoot _configurationRoot;

    public ConfigurationProvider()
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json");

        LoadEnvironmentSpecificAppSettings(configurationBuilder);

        _configurationRoot = configurationBuilder.Build();
    }

    [ExcludeFromCodeCoverage]
    internal ConfigurationProvider(IConfigurationRoot configurationRoot) => _configurationRoot = configurationRoot;

    protected override string RetrieveConfigurationSettingValueThrowIfMissingCore(string key)
    {
        var valueAsRetrieved = _configurationRoot["AppSettings:" + key];

        if (valueAsRetrieved == null)
        {
            throw new ConfigurationSettingException($"The Configuration Setting with Key: {key}, is Missing from the Configuration file in json object: {{  \"Appsettings\": }} ");
        }
        else if (valueAsRetrieved.Length == 0)
        {
            throw new ConfigurationSettingException($"The Configuration Setting with Key: {key}, Exists but its value is Empty");
        }
        else if (IsWhiteSpaces(valueAsRetrieved))
        {
            throw new ConfigurationSettingException($"The Configuration Setting with Key: {key}, Exists but its value is White spaces.");
        }

        return valueAsRetrieved!;
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

    private static void LoadEnvironmentSpecificAppSettings(ConfigurationBuilder configurationBuilder)
    {
        var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var environmentBasedSettingsFile = $"appsettings.{aspNetCoreEnvironment}.json";
        if (File.Exists(environmentBasedSettingsFile))
        {
            configurationBuilder.AddJsonFile(environmentBasedSettingsFile);
        }
    }
}
