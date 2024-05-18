using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Api.Tests.Extensions;

namespace Omini.Opme.Be.Api.Tests.Controllers;

public class PhysiciansControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreatePhysician_When_ValidDataProvided()
    {
        //arrange
        var fakePhysician = PhysicianFaker.GetFakePhysicianCreateDto();

        //act
        var response = await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakePhysician);
        var physicianOutputDto = (await response.GetJsonAsync<ResponseDto<PhysicianOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakePhysician.Should().BeEquivalentTo(physicianOutputDto,
            options =>
                options.Excluding(p => p.Id)
        );
    }

    [Fact]
    public async void Should_UpdatePhysician_When_ValidDataProvided()
    {
        //arrange
        var fakePhysician = PhysicianFaker.GetFakePhysicianCreateDto();

        //act
        var physician = (await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakePhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>()).Data;

        var physicianUpdateDto = PhysicianFaker.GetFakePhysicianUpdateDto(physician.Id);

        var updatePhysicianResponse = await TestClient.Request($"/api/physicians/{physician.Id}").AsAuthenticated().PutJsonAsync(physicianUpdateDto);
        var physicianAfterUpdate = (await TestClient.Request($"/api/physicians/{physician.Id}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<PhysicianOutputDto>>()).Data;

        //assert
        updatePhysicianResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        physicianAfterUpdate.Should().BeEquivalentTo(physicianUpdateDto);
    }   

    [Fact]
    public async void Should_DeletePhysician_When_ValidDataProvided()
    {
        //arrange
        var fakePhysician = PhysicianFaker.GetFakePhysicianCreateDto();

        //act
        var physician = (await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakePhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>()).Data;

        var deleteHospitalResponse = await TestClient.Request($"/api/physicians/{physician.Id}").AsAuthenticated().DeleteAsync();
        var physicianAfterDeleteResponse = await TestClient.Request($"/api/physicians/{physician.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteHospitalResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        physicianAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetPhysicianById_When_ValidDataProvided()
    {
        //arrange
        var fakePhysician = PhysicianFaker.GetFakePhysicianCreateDto();

        //act
        var physician = (await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakePhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>()).Data;
        var physicianResponse = await TestClient.Request($"/api/physicians/{physician.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var physicianData = (await physicianResponse.GetJsonAsync<ResponseDto<PhysicianOutputDto>>()).Data;

        //assert
        physicianResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        physicianData.Should().BeEquivalentTo(physician);
    }

    [Fact]
    public async void Should_GetPhysicians_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstPhysician = PhysicianFaker.GetFakePhysicianCreateDto();
        var fakeSecondPhysician = PhysicianFaker.GetFakePhysicianCreateDto();

        //act
        var firstPhysician = (await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakeFirstPhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>()).Data;
        var secondPhysician = (await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakeSecondPhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>()).Data;

        var physiciansResponse = await TestClient.Request($"/api/physicians").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var physiciansData = (await physiciansResponse.GetJsonAsync<ResponseDto<List<PhysicianOutputDto>>>()).Data;

        //assert
        physiciansResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        physiciansData.Should().Contain(firstPhysician).And.Contain(secondPhysician);
    }
}