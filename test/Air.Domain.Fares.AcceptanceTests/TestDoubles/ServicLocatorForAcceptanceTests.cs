// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain.Fares.AcceptanceTests.TestDoubles
{
    internal class ServicLocatorForAcceptanceTests : ServiceLocatorBase
    {
        protected override ConfigurationProviderBase CreateConfigurationProviderCore() => throw new NotImplementedException();
        protected override HttpMessageHandler CreateHttpMessageHandlerCore() => throw new NotImplementedException();
        protected override RyanairGateway CreateRyanirGatewayCore() => throw new NotImplementedException();
    }
}
