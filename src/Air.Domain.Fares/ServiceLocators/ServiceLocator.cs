//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MIT license.

//using Air.Domain.Fares.ConfigurationProviders;
//using Air.Domain.Fares.Services.Ryanair;

//internal sealed class ServiceLocator : ServiceLocatorBase
//{
//    protected override ConfigurationProviderBase CreateConfigurationProviderCore()
//    {
//        return new ConfigurationProvider();
//    }

//    protected override HttpMessageHandler CreateHttpMessageHandlerCore()
//    {
//        return new SocketsHttpHandler()
//        {
//            PooledConnectionLifetime = TimeSpan.FromMinutes(15),
//        };
//    }

//    protected override RyanairClient CreateImdbServiceGatewayCore()
//    {
//        return new RyanairClient(CreateHttpMessageHandlerCore);
//    }
//}
