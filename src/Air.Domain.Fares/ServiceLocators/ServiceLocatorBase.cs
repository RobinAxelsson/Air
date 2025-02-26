// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain.Fares.ConfigurationProviders;
using Air.Domain.Fares.SegregatedInterfaces;
using Air.Domain.Fares.Services.Ryanair;

internal abstract class ServiceLocatorBase : IHttpMessageHandlerProvider
{
    public RyanairServiceGateway CreateImdbServiceGateway()
    {
        return CreateImdbServiceGatewayCore();
    }

    public ConfigurationProviderBase CreateConfigurationProvider()
    {
        return CreateConfigurationProviderCore();
    }

    public HttpMessageHandler CreateHttpMessageHandler()
    {
        return CreateHttpMessageHandlerCore();
    }

    protected abstract HttpMessageHandler CreateHttpMessageHandlerCore();

    protected abstract ConfigurationProviderBase CreateConfigurationProviderCore();

    protected abstract RyanairServiceGateway CreateImdbServiceGatewayCore();
}
