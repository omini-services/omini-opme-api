using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Api.Tests.Extensions;

namespace Omini.Opme.Api.Tests.Controllers.V1;

public class HospitalsV1ControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreateHospital_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateCommand();

        //act
        var response = await TestClient.Request("/api/v1/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital);
        var hospitalOutputDto = (await response.GetJsonAsync<ResponseDto<HospitalOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakeHospital.Should().BeEquivalentTo(hospitalOutputDto,
            options =>
                options.Excluding(p => p.Code)
        );
    }

    [Fact]
    public async void Should_UpdateHospital_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateCommand();

        //act
        var hospital = (await TestClient.Request("/api/v1/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        var hospitalUpdateCommand = HospitalFaker.GetFakeHospitalUpdateCommand(hospital.Code);

        var updateHospitalResponse = await TestClient.Request($"/api/v1/hospitals/{hospital.Code}").AsAuthenticated().PutJsonAsync(hospitalUpdateCommand);
        var updateHospitalData = (await updateHospitalResponse.GetJsonAsync<ResponseDto<HospitalOutputDto>>()).Data;
        var hospitalAfterUpdate = (await TestClient.Request($"/api/v1/hospitals/{hospital.Code}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        //assert
        updateHospitalResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        hospitalAfterUpdate.Should().BeEquivalentTo(updateHospitalData);
        hospitalUpdateCommand.Should().BeEquivalentTo(updateHospitalData);
    }

    [Fact]
    public async void Should_DeleteHospital_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateCommand();

        //act
        var hospital = (await TestClient.Request("/api/v1/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        var deleteHospitalResponse = await TestClient.Request($"/api/v1/hospitals/{hospital.Code}").AsAuthenticated().DeleteAsync();
        var deleteHospitalData = (await deleteHospitalResponse.GetJsonAsync<ResponseDto<HospitalOutputDto>>()).Data;
        var hospitalAfterDelete = await TestClient.Request($"/api/v1/hospitals/{hospital.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteHospitalResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        fakeHospital.Should().BeEquivalentTo(deleteHospitalData,
            options =>
                options.Excluding(p => p.Code)
        );

        hospitalAfterDelete.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetHospitalById_When_ValidDataProvided()
    {
        //arrange
        var fakeHospital = HospitalFaker.GetFakeHospitalCreateCommand();

        //act
        var hospital = (await TestClient.Request("/api/v1/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;
        var hospitalResponse = await TestClient.Request($"/api/v1/hospitals/{hospital.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var hospitalData = (await hospitalResponse.GetJsonAsync<ResponseDto<HospitalOutputDto>>()).Data;

        //assert
        hospitalResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        hospitalData.Should().BeEquivalentTo(hospital);
    }

    [Fact]
    public async void Should_GetHospitals_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstHospital = HospitalFaker.GetFakeHospitalCreateCommand();
        var fakeSecondHospital = HospitalFaker.GetFakeHospitalCreateCommand();

        //act
        var firstHospital = (await TestClient.Request("/api/v1/hospitals").AsAuthenticated().PostJsonAsync(fakeFirstHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;
        var secondHospital = (await TestClient.Request("/api/v1/hospitals").AsAuthenticated().PostJsonAsync(fakeSecondHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>()).Data;

        var hospitalsResponse = await TestClient.Request($"/api/v1/hospitals").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var hospitalsData = (await hospitalsResponse.GetJsonAsync<ResponseDto<List<HospitalOutputDto>>>()).Data;

        //assert
        hospitalsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        hospitalsData.Should().Contain(firstHospital).And.Contain(secondHospital);
    }
}