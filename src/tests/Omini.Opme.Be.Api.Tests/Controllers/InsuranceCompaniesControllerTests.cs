using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Api.Tests.Extensions;

namespace Omini.Opme.Be.Api.Tests.Controllers;

public class InsuranceCompaniesControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreateInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateDto();

        //act
        var response = await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany);
        var insuranceCompanyOutputDto = (await response.GetJsonAsync<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakeInsuranceCompany.Should().BeEquivalentTo(insuranceCompanyOutputDto,
            options =>
                options.Excluding(p => p.Id)
        );
    }

    [Fact]
    public async void Should_UpdateInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateDto();

        //act
        var insuranceCompany = (await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        var insuranceCompanyUpdateDto = InsuranceCompanyFaker.GetFakeInsuranceCompanyUpdateDto(insuranceCompany.Id);

        var updateInsuranceCompanyResponse = await TestClient.Request($"/api/insurancecompanies/{insuranceCompany.Id}").AsAuthenticated().PutJsonAsync(insuranceCompanyUpdateDto);
        var insuranceCompanyAfterUpdate = (await TestClient.Request($"/api/insurancecompanies/{insuranceCompany.Id}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        //assert
        updateInsuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        insuranceCompanyAfterUpdate.Should().BeEquivalentTo(insuranceCompanyUpdateDto);
    }   

    [Fact]
    public async void Should_DeleteInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateDto();

        //act
        var insuranceCompany = (await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        var deleteInsuranceCompanyResponse = await TestClient.Request($"/api/insurancecompanies/{insuranceCompany.Id}").AsAuthenticated().DeleteAsync();
        var insuranceCompanyAfterDeleteResponse = await TestClient.Request($"/api/insurancecompanies/{insuranceCompany.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteInsuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        insuranceCompanyAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetInsuranceCompaniesById_When_ValidDataProvided()
    {
        //arrange
        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateDto();

        //act
        var insuranceCompany = (await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;
        var insuranceCompanyResponse = await TestClient.Request($"/api/insurancecompanies/{insuranceCompany.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var insuranceCompanyData = (await insuranceCompanyResponse.GetJsonAsync<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        //assert
        insuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        insuranceCompanyData.Should().BeEquivalentTo(insuranceCompany);
    }

    [Fact]
    public async void Should_GetInsuranceCompanies_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateDto();
        var fakeSecondInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateDto();

        //act
        var firstInsuranceCompany = (await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeFirstInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;
        var secondInsuranceCompany = (await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeSecondInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>()).Data;

        var insuranceCompanyResponse = await TestClient.Request($"/api/insurancecompanies").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var insuranceCompanyData = (await insuranceCompanyResponse.GetJsonAsync<ResponseDto<List<InsuranceCompanyOutputDto>>>()).Data;

        //assert
        insuranceCompanyResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        insuranceCompanyData.Should().Contain(firstInsuranceCompany).And.Contain(secondInsuranceCompany);
    }
}