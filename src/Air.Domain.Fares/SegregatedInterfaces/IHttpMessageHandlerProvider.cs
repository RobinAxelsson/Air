using System.Net.Http;
namespace Air.Domain.Fares.SegregatedInterfaces;
internal interface IHttpMessageHandlerProvider
{
    HttpMessageHandler CreateHttpMessageHandler();
}
