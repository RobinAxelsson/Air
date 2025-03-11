//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MIT license.

namespace Air.Domain;
internal sealed class ServiceLocator : ServiceLocatorBase
{
    protected override ConfigurationProviderBase CreateConfigurationProviderCore()
    {
        return new ConfigurationProvider();
    }

    protected override HttpMessageHandler CreateHttpMessageHandlerCore()
    {
        return new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15),
        };
    }

    protected override RyanairServiceGateway CreateRyanirGatewayCore()
    {
        return new RyanairServiceGateway(CreateHttpMessageHandlerCore, CreateConfigurationProvider().GetRyanairServiceBaseUrl());
    }
}
