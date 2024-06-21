using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Api.Tests.Extensions;

namespace Omini.Opme.Api.Tests.Controllers.V1;

public class InsuranceCompaniesV1ControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreateInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateCommand();

        //act
        var response = await TestClient.Request("/api/v1/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany);
        var insuranceCompanyOutputDto = (await response.GetJsonAsync<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakeInsuranceCompany.Should().BeEquivalentTo(insuranceCompanyOutputDto,
            options =>
                options.Excluding(p => p.Code)
        );
    }

    [Fact]
    public async void Should_UpdateInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateCommand();

        //act
        var insuranceCompany = (await TestClient.Request("/api/v1/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        var insuranceCompanyUpdateCommand = InsuranceCompanyFaker.GetFakeInsuranceCompanyUpdateCommand(insuranceCompany.Code);

        var updateInsuranceCompanyResponse = await TestClient.Request($"/api/v1/insurancecompanies/{insuranceCompany.Code}").AsAuthenticated().PutJsonAsync(insuranceCompanyUpdateCommand);
        var updateInsuranceCompanyData = (await updateInsuranceCompanyResponse.GetJsonAsync<ResponseDto<InsuranceCompanyOutputDto>>()).Data;
        var insuranceCompanyAfterUpdate = (await TestClient.Request($"/api/v1/insurancecompanies/{insuranceCompany.Code}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        //assert
        updateInsuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        insuranceCompanyAfterUpdate.Should().BeEquivalentTo(updateInsuranceCompanyData);
        insuranceCompanyUpdateCommand.Should().BeEquivalentTo(updateInsuranceCompanyData);
    }

    [Fact]
    public async void Should_DeleteInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateCommand();

        //act
        var insuranceCompany = (await TestClient.Request("/api/v1/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        var deleteInsuranceCompanyResponse = await TestClient.Request($"/api/v1/insurancecompanies/{insuranceCompany.Code}").AsAuthenticated().DeleteAsync();
        var deleteInsuranceCompanyData = (await deleteInsuranceCompanyResponse.GetJsonAsync<ResponseDto<InsuranceCompanyOutputDto>>()).Data;
        var insuranceCompanyAfterDeleteResponse = await TestClient.Request($"/api/v1/insurancecompanies/{insuranceCompany.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteInsuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        fakeInsuranceCompany.Should().BeEquivalentTo(deleteInsuranceCompanyData,
            options =>
                options.Excluding(p => p.Code)
        );
        
        insuranceCompanyAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetInsuranceCompaniesById_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateCommand();

        //act
        var insuranceCompany = (await TestClient.Request("/api/v1/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;
        var insuranceCompanyResponse = await TestClient.Request($"/api/v1/insurancecompanies/{insuranceCompany.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var insuranceCompanyData = (await insuranceCompanyResponse.GetJsonAsync<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        //assert
        insuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        insuranceCompanyData.Should().BeEquivalentTo(insuranceCompany);
    }

    [Fact]
    public async void Should_GetInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateCommand();
        var fakeSecondInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateCommand();

        //act
        var firstInsuranceCompany = (await TestClient.Request("/api/v1/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeFirstInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;
        var secondInsuranceCompany = (await TestClient.Request("/api/v1/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeSecondInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        var insuranceCompanyResponse = await TestClient.Request($"/api/v1/insurancecompanies").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var insuranceCompanyData = (await insuranceCompanyResponse.GetJsonAsync<ResponsePagedDto<InsuranceCompanyOutputDto>>()).Data;

        //assert
        insuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        insuranceCompanyData.Should().Contain(firstInsuranceCompany).And.Contain(secondInsuranceCompany);
    }
}