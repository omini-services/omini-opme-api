using Bogus;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Api.Tests.Extensions;
using Omini.Opme.Business.Commands;
using Omini.Opme.Domain.Common;

namespace Omini.Opme.Api.Tests.Controllers.V1;

public class QuotationsV1ControllerTests : IntegrationTest
{
    private ResponseDto<HospitalOutputDto> hospitalOutputDto;
    private ResponseDto<PatientOutputDto> patientOutputDto;
    private ResponseDto<InsuranceCompanyOutputDto> insuranceCompanyOutputDto;
    private ResponseDto<PhysicianOutputDto> physicianOutputDto;
    private List<ResponseDto<ItemOutputDto>> itemOutputDtos = new();

    [Fact]
    public async void Should_CreateQuotation_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var faker = new Faker();

        var quotationCreateCommand = QuotationFaker.GetFakeQuotationCreateCommand(itemOutputDtos);
        quotationCreateCommand.HospitalCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.HospitalName = hospitalOutputDto.Data.TradeName;

        quotationCreateCommand.InsuranceCompanyCode = insuranceCompanyOutputDto.Data.Code;
        quotationCreateCommand.InsuranceCompanyName = insuranceCompanyOutputDto.Data.TradeName;

        quotationCreateCommand.PhysicianCode = physicianOutputDto.Data.Code;
        quotationCreateCommand.PhysicianFirstName = physicianOutputDto.Data.FirstName;
        quotationCreateCommand.PhysicianLastName = physicianOutputDto.Data.LastName;

        quotationCreateCommand.PatientCode = patientOutputDto.Data.Code;
        quotationCreateCommand.PatientFirstName = patientOutputDto.Data.FirstName;
        quotationCreateCommand.PatientLastName = patientOutputDto.Data.LastName;

        quotationCreateCommand.PayingSourceType = PayingSourceType.Hospital;
        quotationCreateCommand.PayingSourceCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.PayingSourceName = hospitalOutputDto.Data.TradeName;

        //act
        var response = await TestClient.Request("/api/v1/quotations").AsAuthenticated().PostJsonAsync(quotationCreateCommand);
        var quotationOutputDto = (await response.GetJsonAsync<ResponseDto<QuotationOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        quotationCreateCommand.Should().BeEquivalentTo(quotationOutputDto,
            options =>
                options.Excluding(p => p.Id)
                    .For(p => p.Items)
                    .Exclude(p => p.LineTotal)
                    .For(p => p.Items)
                    .Exclude(p => p.LineId)
                    .For(p => p.Items)
                    .Exclude(p => p.LineOrder)
                    .ExcludingMissingMembers()
        );

        quotationOutputDto.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.LineTotal.Should().BeGreaterThanOrEqualTo(0));

        var total = quotationCreateCommand.Items.Sum(p => p.UnitPrice * p.Quantity);
        quotationOutputDto.Total.Should().Be(total);
        quotationOutputDto.Items.Should().Contain(p => p.ItemName == itemOutputDtos[0].Data.Name);
    }

    [Fact]
    public async void Should_DeleteQuotation_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var faker = new Faker();

        var quotationCreateCommand = QuotationFaker.GetFakeQuotationCreateCommand(itemOutputDtos);
        quotationCreateCommand.HospitalCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.HospitalName = hospitalOutputDto.Data.TradeName;

        quotationCreateCommand.InsuranceCompanyCode = insuranceCompanyOutputDto.Data.Code;
        quotationCreateCommand.InsuranceCompanyName = insuranceCompanyOutputDto.Data.TradeName;

        quotationCreateCommand.PhysicianCode = physicianOutputDto.Data.Code;
        quotationCreateCommand.PhysicianFirstName = physicianOutputDto.Data.FirstName;
        quotationCreateCommand.PhysicianLastName = physicianOutputDto.Data.LastName;

        quotationCreateCommand.PatientCode = patientOutputDto.Data.Code;
        quotationCreateCommand.PatientFirstName = patientOutputDto.Data.FirstName;
        quotationCreateCommand.PatientLastName = patientOutputDto.Data.LastName;

        quotationCreateCommand.PayingSourceType = PayingSourceType.Hospital;
        quotationCreateCommand.PayingSourceCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.PayingSourceName = hospitalOutputDto.Data.TradeName;


        //act
        var quotation = (await TestClient.Request("/api/v1/quotations").AsAuthenticated().PostJsonAsync(quotationCreateCommand).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;

        var deleteQuotationResponse = await TestClient.Request($"/api/v1/quotations/{quotation.Id}").AsAuthenticated().DeleteAsync();
        var deleteQuotationData = (await deleteQuotationResponse.GetJsonAsync<ResponseDto<QuotationOutputDto>>()).Data;
        var quotationAfterDeleteResponse = await TestClient.Request($"/api/v1/quotations/{quotation.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteQuotationResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        quotationAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_CreateQuotationItem_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var quotationCreateCommand = QuotationFaker.GetFakeQuotationCreateCommand(itemOutputDtos);
        quotationCreateCommand.HospitalCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.HospitalName = hospitalOutputDto.Data.TradeName;

        quotationCreateCommand.InsuranceCompanyCode = insuranceCompanyOutputDto.Data.Code;
        quotationCreateCommand.InsuranceCompanyName = insuranceCompanyOutputDto.Data.TradeName;

        quotationCreateCommand.PhysicianCode = physicianOutputDto.Data.Code;
        quotationCreateCommand.PhysicianFirstName = physicianOutputDto.Data.FirstName;
        quotationCreateCommand.PhysicianLastName = physicianOutputDto.Data.LastName;

        quotationCreateCommand.PatientCode = patientOutputDto.Data.Code;
        quotationCreateCommand.PatientFirstName = patientOutputDto.Data.FirstName;
        quotationCreateCommand.PatientLastName = patientOutputDto.Data.LastName;
        
        quotationCreateCommand.PayingSourceType = PayingSourceType.Hospital;
        quotationCreateCommand.PayingSourceCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.PayingSourceName = hospitalOutputDto.Data.TradeName;

        var quotation = (await TestClient.Request("/api/v1/quotations").AsAuthenticated().PostJsonAsync(quotationCreateCommand).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;

        var fakeItem = ItemFaker.GetFakerItemCreateCommand().Generate();
        var newItem = (await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        //act        
        var faker = new Faker();
        var quotationCreateItemCommand = new CreateQuotationItemCommand()
        {
            QuotationId = quotation.Id,
            ItemCode = newItem.Code,
            Quantity = faker.Random.Number(0, 100),
            UnitPrice = Math.Round(faker.Random.Decimal(0, 100), 2),
        };

        var updateQuotationResponse = await TestClient.Request($"/api/v1/quotations/{quotation.Id}/items").AsAuthenticated().PostJsonAsync(quotationCreateItemCommand);
        var quotationAfterUpdate = (await TestClient.Request($"/api/v1/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>()).Data;

        //assert
        updateQuotationResponse.StatusCode.Should().Be(StatusCodes.Status201Created);

        quotationAfterUpdate.Items.Should().HaveCount(quotation.Items.Count + 1);

        quotationAfterUpdate.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.LineTotal.Should().BeGreaterThanOrEqualTo(0));

        var addedItem = quotationAfterUpdate.Items.Last();
        quotationAfterUpdate.Items.Should().ContainEquivalentOf(addedItem);

        var total = quotationCreateCommand.Items.Sum(p => p.UnitPrice * p.Quantity) + (quotationCreateItemCommand.UnitPrice * quotationCreateItemCommand.Quantity);
        quotationAfterUpdate.Total.Should().Be(total);
        quotationAfterUpdate.Items.Should().Contain(p => p.ItemName == newItem.Name);
    }

    [Fact]
    public async void Should_UpdateQuotationItem_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var quotationCreateCommand = QuotationFaker.GetFakeQuotationCreateCommand(itemOutputDtos);
        quotationCreateCommand.HospitalCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.HospitalName = hospitalOutputDto.Data.TradeName;
        
        quotationCreateCommand.InsuranceCompanyCode = insuranceCompanyOutputDto.Data.Code;
        quotationCreateCommand.InsuranceCompanyName = insuranceCompanyOutputDto.Data.TradeName;
        
        quotationCreateCommand.PhysicianCode = physicianOutputDto.Data.Code;
        quotationCreateCommand.PhysicianFirstName = physicianOutputDto.Data.FirstName;
        quotationCreateCommand.PhysicianLastName = physicianOutputDto.Data.LastName;

        quotationCreateCommand.PatientCode = patientOutputDto.Data.Code;
        quotationCreateCommand.PatientFirstName = patientOutputDto.Data.FirstName;
        quotationCreateCommand.PatientLastName = patientOutputDto.Data.LastName;

        quotationCreateCommand.PayingSourceType = PayingSourceType.Hospital;
        quotationCreateCommand.PayingSourceCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.PayingSourceName = hospitalOutputDto.Data.TradeName;

        var quotation = (await TestClient.Request("/api/v1/quotations").AsAuthenticated().PostJsonAsync(quotationCreateCommand).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;
        var lineIdToUpdate = quotation.Items[0].LineId;

        var totalCreate = quotationCreateCommand.Items.Sum(p => p.UnitPrice * p.Quantity);

        //act        
        var quotationUpdateItemCommand = new UpdateQuotationItemCommand()
        {
            QuotationId = quotation.Id,
            LineId = lineIdToUpdate,
            LineOrder = quotation.Items[0].LineOrder,
            ItemCode = quotation.Items[1].ItemCode,
            Quantity = quotation.Items[1].Quantity,
            UnitPrice = quotation.Items[1].UnitPrice,
        };

        var updateQuotationResponse = await TestClient.Request($"/api/v1/quotations/{quotation.Id}/items/{quotation.Items[0].LineId}").AsAuthenticated().PutJsonAsync(quotationUpdateItemCommand);
        var quotationAfterUpdate = (await TestClient.Request($"/api/v1/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>()).Data;

        //assert
        updateQuotationResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        var updatedItem = quotationAfterUpdate.Items.Single(p => p.LineId == lineIdToUpdate);

        quotationAfterUpdate.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.LineTotal.Should().BeGreaterThanOrEqualTo(0));

        var total = totalCreate - (quotation.Items[0].Quantity * quotation.Items[0].UnitPrice) + (quotation.Items[1].Quantity * quotation.Items[1].UnitPrice);
        quotationAfterUpdate.Total.Should().Be(total);
        updatedItem.ItemName.Should().Be(itemOutputDtos[1].Data.Name);
    }

    [Fact]
    public async void Should_DeleteQuotationItem_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var quotationCreateCommand = QuotationFaker.GetFakeQuotationCreateCommand(itemOutputDtos);
        quotationCreateCommand.HospitalCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.HospitalName = hospitalOutputDto.Data.TradeName;
        quotationCreateCommand.InsuranceCompanyCode = insuranceCompanyOutputDto.Data.Code;
        quotationCreateCommand.InsuranceCompanyName = insuranceCompanyOutputDto.Data.TradeName;
        quotationCreateCommand.PhysicianCode = physicianOutputDto.Data.Code;
        quotationCreateCommand.PhysicianFirstName = physicianOutputDto.Data.FirstName;
        quotationCreateCommand.PhysicianLastName = physicianOutputDto.Data.LastName;
        quotationCreateCommand.PatientCode = patientOutputDto.Data.Code;
        quotationCreateCommand.PatientFirstName = patientOutputDto.Data.FirstName;
        quotationCreateCommand.PatientLastName = patientOutputDto.Data.LastName;
        quotationCreateCommand.PayingSourceType = PayingSourceType.Hospital;
        quotationCreateCommand.PayingSourceCode = hospitalOutputDto.Data.Code;
        quotationCreateCommand.PayingSourceName = hospitalOutputDto.Data.TradeName;

        var quotation = (await TestClient.Request("/api/v1/quotations").AsAuthenticated().PostJsonAsync(quotationCreateCommand).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;

        //act

        var deleteQuotationItemResponse = await TestClient.Request($"/api/v1/quotations/{quotation.Id}/items/{quotation.Items[0].LineId}").AsAuthenticated().DeleteAsync();
        var quotationAfterDeleteItemResponse = await TestClient.Request($"/api/v1/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>();
        var quotationAfterUpdateItem = quotationAfterDeleteItemResponse.Data;

        //assert
        deleteQuotationItemResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        quotationAfterUpdateItem.Items.Should().HaveCount(quotation.Items.Count - 1);

        quotationAfterUpdateItem.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.LineTotal.Should().BeGreaterThanOrEqualTo(0));

        quotationAfterUpdateItem.Items.Should().NotContain(quotation.Items[0]);
    }

    private async Task Seed()
    {
        var fakeItems = ItemFaker.GetFakerItemCreateCommand().Generate(5);
        foreach (var fakeItem in fakeItems)
        {
            var response = await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>();
            itemOutputDtos.Add(response);
        }

        var fakeHospital = HospitalFaker.GetFakeHospitalCreateCommand();
        hospitalOutputDto = await TestClient.Request("/api/v1/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>();

        var fakePatient = PatientFaker.GetFakePatientCreateCommand();
        patientOutputDto = await TestClient.Request("/api/v1/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>();

        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateCommand();
        insuranceCompanyOutputDto = await TestClient.Request("/api/v1/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>();

        var fakePhysician = PhysicianFaker.GetFakePhysicianCreateCommand();
        physicianOutputDto = await TestClient.Request("/api/v1/physicians").AsAuthenticated().PostJsonAsync(fakePhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>();
    }
}