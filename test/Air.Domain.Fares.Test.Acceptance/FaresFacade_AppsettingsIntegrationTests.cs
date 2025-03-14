// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain.Fares.Test.Acceptance.Helpers;
using Testing.Shared;

namespace Air.Domain.Fares.Origin.Acceptance;

public class FaresFacade_AppsettingsIntegrationTests
{
    //ConfigurationProvider should get initialized in every facade method
    private static async Task InitializeApplicationConfiguration_SyncFlightFares()
    {
        var irrelevantTripSpec = FlightSpecDtoFactory.ValidStub();
        using var domainFacade = new FaresFacade();
        await domainFacade.SyncFlightFares(irrelevantTripSpec);
    }

    //We set ASPNETCORE_ENVIRONMENT to override the default appsettings with different files in each test
    //Because the tests run in parallell by default we need to set NotInParallell
    //When needed move to its own assembly with similar tests
    private Action SetAppsettingsEnvironment(string value)
    {
        const string asp_env = "ASPNETCORE_ENVIRONMENT";

        var current = Environment.GetEnvironmentVariable(asp_env);
        if (!string.IsNullOrEmpty(current)) throw new MoreThenOneTestAccessAspEnvironment($"{asp_env} was not cleaned up before running test, it is set to '{current}'");

        Environment.SetEnvironmentVariable(asp_env, value);
        return () => Environment.SetEnvironmentVariable(asp_env, "");
    }

    [Category("Acceptance Test")]
    [Test, NotInParallel(Order = 1)]
    public async Task InitializeConnectionString_test1WithInvalidConnectionString_Throws()
    {
      
        var reset = SetAppsettingsEnvironment("test1");
        try
        {
            await InitializeApplicationConfiguration_SyncFlightFares();
            Assert.Fail($"We were expecting an {nameof(InvalidConnectionStringException)} expection to be thrown but no exception was thrown");
        }
        catch (InvalidConnectionStringException e)
        {
            AssertEx.EnsureExceptionMessageContains(e, ["Port required", "Password", "TrustServerCertificate", "UserId"]);
        }
        finally
        {
            reset();
        }
    }

    [Category("Acceptance Test")]
    [Test, NotInParallel(Order = 1)]
    public async Task InitializeConfigurationSetting_test2WithNoForwardSlashUrl_Throws()
    {

        var reset = SetAppsettingsEnvironment("test2");

        try
        {
            await InitializeApplicationConfiguration_SyncFlightFares();
            Assert.Fail($"We were expecting an {nameof(ConfigurationSettingInvalidException)} expection to be thrown but no exception was thrown");
        }
        catch (ConfigurationSettingInvalidException e)
        {
            AssertEx.EnsureExceptionMessageContains(e, "forward slash");
        }
        finally
        {
            reset();
        }
    }

    [Category("Acceptance Test")]
    [Test, NotInParallel(Order = 1)]
    public async Task InitializeConfigurationSetting_test3WithInvalidRyanairPath_Throws()
    {

        var reset = SetAppsettingsEnvironment("test3");

        try
        {
            await InitializeApplicationConfiguration_SyncFlightFares();
            Assert.Fail($"We were expecting an {nameof(RyanairServiceRequestException)} expection to be thrown but no exception was thrown");
        }
        catch (RyanairServiceRequestException e)
        {
            AssertEx.EnsureExceptionMessageContains(e, "\"Not Found\"");
        }
        finally
        {
            reset();
        }
    }
}
