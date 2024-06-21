using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Api.Tests.Extensions;


namespace Omini.Opme.Api.Tests.Controllers.V1;

public class PatientsV1ControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreatePatient_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateCommand();

        //act
        var response = await TestClient.Request("/api/v1/patients").AsAuthenticated().PostJsonAsync(fakePatient);
        var patientOutputDto = (await response.GetJsonAsync<ResponseDto<PatientOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakePatient.Should().BeEquivalentTo(patientOutputDto,
            options =>
                options.Excluding(p => p.Code)
        );
    }

    [Fact]
    public async void Should_UpdatePatient_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateCommand();

        //act
        var patient = (await TestClient.Request("/api/v1/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        var patientUpdateCommand = PatientFaker.GetFakePatientUpdateCommand(patient.Code);

        var updatePatientResponse = await TestClient.Request($"/api/v1/patients/{patient.Code}").AsAuthenticated().PutJsonAsync(patientUpdateCommand);
        var updatePatientData = (await updatePatientResponse.GetJsonAsync<ResponseDto<PatientOutputDto>>()).Data;
        var patientAfterUpdate = (await TestClient.Request($"/api/v1/patients/{patient.Code}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        //assert
        updatePatientResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        patientAfterUpdate.Should().BeEquivalentTo(updatePatientData);
        patientUpdateCommand.Should().BeEquivalentTo(updatePatientData);
    }   

    [Fact]
    public async void Should_DeletePatient_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateCommand();

        //act
        var patient = (await TestClient.Request("/api/v1/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        var deletePatientResponse = await TestClient.Request($"/api/v1/patients/{patient.Code}").AsAuthenticated().DeleteAsync();
        var deletePatientData = (await deletePatientResponse.GetJsonAsync<ResponseDto<PatientOutputDto>>()).Data;
        var patientAfterDeleteResponse = await TestClient.Request($"/api/v1/patients/{patient.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deletePatientResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        fakePatient.Should().BeEquivalentTo(deletePatientData,
            options =>
                options.Excluding(p => p.Code)
        );

        patientAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetPatientById_When_ValidDataProvided()
    {
        //arrange
        var fakePatient = PatientFaker.GetFakePatientCreateCommand();

        //act
        var patient = (await TestClient.Request("/api/v1/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;
        var patientResponse = await TestClient.Request($"/api/v1/patients/{patient.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var patientData = (await patientResponse.GetJsonAsync<ResponseDto<PatientOutputDto>>()).Data;

        //assert
        patientResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        patientData.Should().BeEquivalentTo(patient);
    }

    [Fact]
    public async void Should_GetPatients_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstPatient = PatientFaker.GetFakePatientCreateCommand();
        var fakeSecondPatient = PatientFaker.GetFakePatientCreateCommand();

        //act
        var firstPatient = (await TestClient.Request("/api/v1/patients").AsAuthenticated().PostJsonAsync(fakeFirstPatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;
        var secondPatient = (await TestClient.Request("/api/v1/patients").AsAuthenticated().PostJsonAsync(fakeSecondPatient).ReceiveJson<ResponseDto<PatientOutputDto>>()).Data;

        var patientsResponse = await TestClient.Request($"/api/v1/patients").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var patientsData = (await patientsResponse.GetJsonAsync<ResponseDto<List<PatientOutputDto>>>()).Data;

        //assert
        patientsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        patientsData.Should().Contain(firstPatient).And.Contain(secondPatient);
    }
}