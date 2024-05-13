using System.Net;
using FluentAssertions;

namespace Omini.Opme.Be.Api.Tests;

public class QuotationControllerTests : IntegrationTest
{
    [Fact]
    public async void Create_()
    {
        Authenticate();

        //act
        var response = await TestClient.GetAsync("/api/items");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}