using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Api.Tests.Extensions;

namespace Omini.Opme.Be.Api.Tests.Controllers;

public class PatientsControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreatePatient_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateDto();

        //act
        var response = await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakePatient);
        var patientOutputDto = (await response.GetJsonAsync<ResponseDto<PatientOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakePatient.Should().BeEquivalentTo(patientOutputDto,
            options =>
                options.Excluding(p => p.Id)
        );
    }

    [Fact]
    public async void Should_UpdatePatient_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateDto();

        //act
        var patient = (await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        var patientUpdateDto = PatientFaker.GetFakePatientUpdateDto(patient.Id);

        var updatePatientResponse = await TestClient.Request($"/api/patients/{patient.Id}").AsAuthenticated().PutJsonAsync(patientUpdateDto);
        var patientAfterUpdate = (await TestClient.Request($"/api/patients/{patient.Id}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        //assert
        updatePatientResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        patientAfterUpdate.Should().BeEquivalentTo(patientUpdateDto);
    }   

    [Fact]
    public async void Should_DeletePatient_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateDto();

        //act
        var patient = (await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        var deletePatientResponse = await TestClient.Request($"/api/patients/{patient.Id}").AsAuthenticated().DeleteAsync();
        var patientAfterDeleteResponse = await TestClient.Request($"/api/patients/{patient.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deletePatientResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        patientAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetPatientById_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateDto();

        //act
        var patient = (await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;
        var patientResponse = await TestClient.Request($"/api/patients/{patient.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var patientData = (await patientResponse.GetJsonAsync<ResponseDto<PatientOutputDto>>()).Data;

        //assert
        patientResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        patientData.Should().BeEquivalentTo(patient);
    }

    [Fact]
    public async void Should_GetPatients_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstPatient = PatientFaker.GetFakePatientCreateDto();
        var fakeSecondPatient = PatientFaker.GetFakePatientCreateDto();

        //act
        var firstPatient = (await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakeFirstPatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;
        var secondPatient = (await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakeSecondPatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        var patientsResponse = await TestClient.Request($"/api/patients").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var patientsData = (await patientsResponse.GetJsonAsync<ResponseDto<List<PatientOutputDto>>>()).Data;

        //assert
        patientsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        patientsData.Should().Contain(firstPatient).And.Contain(secondPatient);
    }
}