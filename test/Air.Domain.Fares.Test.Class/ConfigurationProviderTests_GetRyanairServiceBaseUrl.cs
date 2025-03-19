using Air.Domain.Fares.Test.Shared.Asserters;
using Microsoft.Extensions.Configuration;

// ReSharper disable ConvertToConstant.Local
// This is because TUnit does not like constants in expected variables

namespace Air.Domain.Fares.Test.Class;

public class ConfigurationProviderTests_GetRyanairServiceBaseUrl
{
    private const string RyanairServiceBaseUrlKey = "RyanairBaseUrl";
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
    [Arguments("https://www.ryanair.com/")]
    [Arguments("https://www.ryanair.com:8888/")]
    [Arguments("http://localhost/")]
    [Arguments("http://localhost:8080/")]
    public async Task GetRyanairServiceBaseUrl_HappyPath_ReturnsCorrectValue(string ryanairBaseUrl)
    {
        // Arrange
        var configurationProvider = CreateConfigurationProvider(RyanairServiceBaseUrlKey, ryanairBaseUrl);

        // Act
        string actualRyanairUrl = configurationProvider.GetRyanairServiceBaseUrl();

        // Assert
        await Assert.That(ryanairBaseUrl).IsEqualTo(actualRyanairUrl).Because("the configuration provider should always return the same url.");
    }

    [Test]
    [Category("Class Test")]
    public void GetRyanairServiceBaseUrl_NoTrailingForwardSlash_Throws()
    {
        // Arrange
        string baseUrlNoForwardSlash = "https://www.ryanair.com";
        var configurationProvider = CreateConfigurationProvider(RyanairServiceBaseUrlKey, baseUrlNoForwardSlash);

        AssertExBuilder.Act(() => configurationProvider.GetRyanairServiceBaseUrl())
            .AssertThrows<InvalidHttpUrlException>(e =>
            {
                AssertEx.EnsureExceptionMessageContains(e, $"forward slash");
            });
    }

    [Test]
    [Category("Class Test")]
    public void GetRyanairServiceBaseUrl_NoHttpsOrHttp_Throws()
    {
        string baseUrlNoForwardSlash = "ftp://www.ryanair.com/";
        var configurationProvider = CreateConfigurationProvider(RyanairServiceBaseUrlKey, baseUrlNoForwardSlash);

        AssertExBuilder.Act(() => configurationProvider.GetRyanairServiceBaseUrl())
            .AssertThrows<InvalidHttpUrlException>(e =>
            {
                AssertEx.EnsureExceptionMessageContains(e, $"http or https is required");
                AssertEx.EnsureExceptionMessageDoesNotContains(e, $"absolute uri");
            });
    }

    [Test]
    [Category("Class Test")]
    public void GetRyanairServiceBaseUrl_NotAbsoluteUri_Throws()
    {
        // Arrange
        string baseUrlNoForwardSlash = "www.ryanair.com/";
        var configurationProvider = CreateConfigurationProvider(RyanairServiceBaseUrlKey, baseUrlNoForwardSlash);

        AssertExBuilder.Act(() => configurationProvider.GetRyanairServiceBaseUrl())
            .AssertThrows<InvalidHttpUrlException>(e =>
            {
                AssertEx.EnsureExceptionMessageContains(e, $"absolute uri");
            });
    }
}
