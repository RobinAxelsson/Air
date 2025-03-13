// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain.Fares.Test.Acceptance.Helpers;

namespace Air.Domain.Fares.Origin.Acceptance;

public class FaresFacade_ConfigurationProviderTests
{
    //ConfigurationProvider should get initialized in every facade method
    private static async Task CallAnyDomainFacadeMethod()
    {
        var irrelevantTripSpec = FlightSpecDtoFactory.ValidStub();
        using var domainFacade = new FaresFacade();
        await domainFacade.SyncFlightFares(irrelevantTripSpec);
    }

    //We set this to override the default appsettings
    private IDisposable SetAppsettingsEnvironment(string value)
    {
        const string environment = "ASPNETCORE_ENVIRONMENT";

        var saved = Environment.GetEnvironmentVariable(environment);
        Environment.SetEnvironmentVariable(environment, value);
        return new DisposeHelper(() => Environment.SetEnvironmentVariable(environment, saved));
    }

    [Category("Acceptance Origin")]
    [Test]
    public async Task InitializeConfigurationSetting_ConnectionString1_Throws()
    {
        using var resetDisposable = SetAppsettingsEnvironment("ConnectionString1");
        await CallAnyDomainFacadeMethod();
    }

    private sealed class DisposeHelper : IDisposable
    {
        private readonly Action _onDispose;

        public DisposeHelper(Action onDispose)
        {
            _onDispose = onDispose;
        }
        public void Dispose() => _onDispose();
    }
}
