using System.Net;

namespace Air.Domain.Fares.Tests.AcceptanceTests.TestDoubles;

internal sealed class HttpMessageHandlerSpy : DelegatingHandler
{
    //private readonly TestMediator _testMediator = testMeditor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(GetFaresFromJson())
        };
        return Task.FromResult(httpResponseMessage);

        //if (_testMediator.ExceptionInformation == null)
        //{
        //    var movies = _testMediator.MoviesForGetAllMovies!;
        //    return WriteMoviesToResponse(request, movies);
        //}
        //else
        //{
        //    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        StatusCode = (HttpStatusCode)_testMediator.ExceptionInformation.ExceptionReason,
        //        Content = new StringContent("Exception Occured")
        //    };

        //    return Task.FromResult(httpResponseMessage);
        //}
    }

    private static string GetFaresFromJson()
    {
        return File.ReadAllText("TestData/booking-response-3-flights.json");
    }
}
