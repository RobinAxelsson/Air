// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace Domain.Fares;
internal abstract class ServiceLocatorBase
{
    //public RyanairClient CreateImdbServiceGateway()
    //{
    //    return CreateImdbServiceGatewayCore();
    //}

    //public ConfigurationProviderBase CreateConfigurationProvider()
    //{
    //    return CreateConfigurationProviderCore();
    //}

    public HttpMessageHandler CreateHttpMessageHandler()
    {
        return CreateHttpMessageHandlerCore();
    }

    protected abstract HttpMessageHandler CreateHttpMessageHandlerCore();

    //protected abstract ConfigurationProviderBase CreateConfigurationProviderCore();

    //protected abstract RyanairClient CreateImdbServiceGatewayCore();
}
