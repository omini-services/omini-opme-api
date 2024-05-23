using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Api.Tests.Extensions;

namespace Omini.Opme.Be.Api.Tests.Controllers;

public class HospitalResponseControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreateHospital_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateDto();

        //act
        var response = await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital);
        var hospitalOutputDto = (await response.GetJsonAsync<ResponseDto<HospitalOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakeHospital.Should().BeEquivalentTo(hospitalOutputDto,
            options =>
                options.Excluding(p => p.Id)
        );
    }

    [Fact]
    public async void Should_UpdateHospital_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateDto();

        //act
        var hospital = (await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        var hospitalUpdateDto = HospitalFaker.GetFakeHospitalUpdateDto(hospital.Id);

        var updateHospitalResponse = await TestClient.Request($"/api/hospitals/{hospital.Id}").AsAuthenticated().PutJsonAsync(hospitalUpdateDto);
        var hospitalAfterUpdate = (await TestClient.Request($"/api/hospitals/{hospital.Id}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        //assert
        updateHospitalResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        hospitalAfterUpdate.Should().BeEquivalentTo(hospitalUpdateDto);
    }   

    [Fact]
    public async void Should_DeleteHospital_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateDto();

        //act
        var hospital = (await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        var deleteHospitalResponse = await TestClient.Request($"/api/hospitals/{hospital.Id}").AsAuthenticated().DeleteAsync();
        var hospitalAfterDeleteResponse = await TestClient.Request($"/api/hospitals/{hospital.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteHospitalResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        hospitalAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetHospitalById_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateDto();

        //act
        var hospital = (await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;
        var hospitalResponse = await TestClient.Request($"/api/hospitals/{hospital.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var hospitalData = (await hospitalResponse.GetJsonAsync<ResponseDto<HospitalOutputDto>>()).Data;

        //assert
        hospitalResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        hospitalData.Should().BeEquivalentTo(hospital);
    }

    [Fact]
    public async void Should_GetHospitals_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstHospital = HospitalFaker.GetFakeHospitalCreateDto();
        var fakeSecondHospital = HospitalFaker.GetFakeHospitalCreateDto();

        //act
        var firstHospital = (await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeFirstHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;
        var secondHospital = (await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeSecondHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        var hospitalsResponse = await TestClient.Request($"/api/hospitals").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var hospitalsData = (await hospitalsResponse.GetJsonAsync<ResponseDto<List<HospitalOutputDto>>>()).Data;

        //assert
        hospitalsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        hospitalsData.Should().Contain(firstHospital).And.Contain(secondHospital);
    }
}