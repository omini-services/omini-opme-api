using System.Net;
using FluentAssertions;

namespace Omini.Opme.Be.Api.Tests;

internal class QuotationControllerTests : IntegrationTest
{
    [Fact]
    public async void Create_()
    {
        //arrange

        //act
        var response = await TestClient.GetAsync("");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}