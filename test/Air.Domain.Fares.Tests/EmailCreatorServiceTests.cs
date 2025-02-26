using Air.Domain.Fares.Enums;
using Air.Domain.Fares.Services.EmailCreator;
using Air.Domain.Fares.Services.EmailCreator.Dtos;
using Xunit.Abstractions;

namespace Air.Domain.Fares.Tests;

public class EmailCreatorServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public EmailCreatorServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateWeekEndEmails_DtoList_ValidHtml()
    {
        // Arrange

        var model = new WeekendSurfViewModelDto
        {
            Airline = Airline.Ryanair,
            DepartureDate = DateTime.Now,
            Destination = Airport.LIS,
            Origin = Airport.GOT,
            Price = 1600
        };
        // Act
        var html = await EmailCreatorService.CreateWeekendEmail(model);
        _testOutputHelper.WriteLine(html);
        // Assert
        //Assert.Contains("<h1>Weekend 1</h1>", html);
        //Assert.Contains("<p>Description 1</p>", html);
        //Assert.Contains("<h1>Weekend 2</h1>", html);
        //Assert.Contains("<p>Description 2</p>", html);
    }
}
