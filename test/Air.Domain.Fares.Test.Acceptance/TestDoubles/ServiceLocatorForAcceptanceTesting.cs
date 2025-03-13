// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Air.Domain.Fares.Test.Acceptance.TestMediators;

namespace Air.Domain.Fares.Test.Acceptance.TestDoubles;

internal class ServiceLocatorForAcceptanceTesting : ServiceLocatorBase
{
    private readonly TestMediator _testMediator;
    public ServiceLocatorForAcceptanceTesting(TestMediator testMediator) => _testMediator = testMediator;
    protected override ConfigurationProviderBase CreateConfigurationProviderCore() => new ConfigurationProvider();
    protected override HttpMessageHandler CreateHttpMessageHandlerCore()
    {
        // For Testing Purposes, you need only return an instance of the HttpMessageHandlerSpy (with the test mediator constructor parameter
        // If during testing there are situations where certain test scenarios require you to "intercept" using the spy
        // while others require you to "forward" the call onto the actual HttpMessageHandler
        // then the delegating handler with the InnerHandler as the actual (SocketsHttpHandler) might be needed
        var socketsHttpHandler = new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        };

        var myDelegatingHander = new HttpMessageHandlerSpy(_testMediator)
        {
            InnerHandler = socketsHttpHandler,
        };

        return myDelegatingHander;
    }

    protected override RyanairServiceGateway CreateRyanirGatewayCore() => new RyanairServiceGateway(CreateHttpMessageHandlerCore, new ConfigurationProvider().GetRyanairServiceBaseUrl());
}
