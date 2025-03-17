using Microsoft.Extensions.Configuration;

namespace Air.Domain.Fares.Test.Class;

public class ConfigurationProviderTests
{
    private static IConfigurationRoot CreateConfigurationRoot(string key, string value)
    {
        var appSettingsDictionary = new Dictionary<string, string?>()
        {
            { "AppSettings:" + key, value },
        };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddInMemoryCollection(appSettingsDictionary);
        return configurationBuilder.Build();
    }

    private static ConfigurationProviderBase CreateConfigurationProvider(string key, string value)
    {
        var configurationRoot = CreateConfigurationRoot(key, value);
        return new ConfigurationProvider(configurationRoot);
    }

    [Test]
    [Category("Class Test")]
    public async Task Run_setting()
    {

    }

}
