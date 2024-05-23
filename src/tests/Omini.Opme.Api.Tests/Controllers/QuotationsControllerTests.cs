using Bogus;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Domain.Common;

namespace Omini.Opme.Api.Tests.Controllers;

public class QuotationsControllerTests : IntegrationTest
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

        var quotationCreateDto = QuotationFaker.GetFakeQuotationCreateDto(itemOutputDtos);
        quotationCreateDto.HospitalId = hospitalOutputDto.Data.Id;
        quotationCreateDto.InsuranceCompanyId = insuranceCompanyOutputDto.Data.Id;
        quotationCreateDto.PhysicianId = physicianOutputDto.Data.Id;
        quotationCreateDto.InternalSpecialistId = Guid.NewGuid();
        quotationCreateDto.PatientId = patientOutputDto.Data.Id;
        quotationCreateDto.PayingSourceType = PayingSourceType.Hospital;
        quotationCreateDto.PayingSourceId = hospitalOutputDto.Data.Id;

        //act
        var response = await TestClient.Request("/api/quotations").AsAuthenticated().PostJsonAsync(quotationCreateDto);
        var quotationOutputDto = (await response.GetJsonAsync<ResponseDto<QuotationOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        quotationCreateDto.Should().BeEquivalentTo(quotationOutputDto,
            options =>
                options.Excluding(p => p.Id)
                    .For(p => p.Items)
                    .Exclude(p => p.ItemTotal)
                    .For(p => p.Items)
                    .Exclude(p => p.LineId)
                    .For(p => p.Items)
                    .Exclude(p => p.LineOrder)
                    .ExcludingMissingMembers()
        );

        quotationOutputDto.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));

        var total = quotationCreateDto.Items.Sum(p => p.UnitPrice * p.Quantity);
        quotationOutputDto.Total.Should().Be(total);
        quotationOutputDto.Items.Should().Contain(p => p.ItemName == itemOutputDtos[0].Data.Name);
    }

    [Fact]
    public async void Should_DeleteQuotation_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var faker = new Faker();

        var quotationCreateDto = QuotationFaker.GetFakeQuotationCreateDto(itemOutputDtos);
        quotationCreateDto.HospitalId = hospitalOutputDto.Data.Id;
        quotationCreateDto.InsuranceCompanyId = insuranceCompanyOutputDto.Data.Id;
        quotationCreateDto.PhysicianId = physicianOutputDto.Data.Id;
        quotationCreateDto.InternalSpecialistId = Guid.NewGuid();
        quotationCreateDto.PatientId = patientOutputDto.Data.Id;
        quotationCreateDto.PayingSourceType = Domain.Enums.PayingSourceType.Hospital;
        quotationCreateDto.PayingSourceId = hospitalOutputDto.Data.Id;

        //act
        var quotation = (await TestClient.Request("/api/quotations").AsAuthenticated().PostJsonAsync(quotationCreateDto).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;

        var deleteQuotationResponse = await TestClient.Request($"/api/quotations/{quotation.Id}").AsAuthenticated().DeleteAsync();
        var quotationAfterDeleteResponse = await TestClient.Request($"/api/quotations/{quotation.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteQuotationResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        quotationAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_CreateQuotationItem_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var quotationCreateDto = QuotationFaker.GetFakeQuotationCreateDto(itemOutputDtos);
        quotationCreateDto.HospitalId = hospitalOutputDto.Data.Id;
        quotationCreateDto.InsuranceCompanyId = insuranceCompanyOutputDto.Data.Id;
        quotationCreateDto.PhysicianId = physicianOutputDto.Data.Id;
        quotationCreateDto.InternalSpecialistId = Guid.NewGuid();
        quotationCreateDto.PatientId = patientOutputDto.Data.Id;
        quotationCreateDto.PayingSourceType = Domain.Enums.PayingSourceType.Hospital;
        quotationCreateDto.PayingSourceId = hospitalOutputDto.Data.Id;

        var quotation = (await TestClient.Request("/api/quotations").AsAuthenticated().PostJsonAsync(quotationCreateDto).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;

        var fakeItem = ItemFaker.GetFakerItemCreateDto().Generate();
        var newItem = (await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        //act        
        var faker = new Faker();
        var quotationCreateItemDto = new QuotationCreateLineItemDto()
        {
            QuotationId = quotation.Id,
            ItemCode = newItem.Code,
            Quantity = faker.Random.Number(0, 100),
            UnitPrice = faker.Random.Double(0, 100),
        };

        var updateQuotationResponse = await TestClient.Request($"/api/quotations/{quotation.Id}/items").AsAuthenticated().PostJsonAsync(quotationCreateItemDto);
        var quotationAfterUpdate = (await TestClient.Request($"/api/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>()).Data;

        //assert
        updateQuotationResponse.StatusCode.Should().Be(StatusCodes.Status201Created);

        quotationAfterUpdate.Items.Should().HaveCount(quotation.Items.Count + 1);

        quotationAfterUpdate.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));

        var addedItem = quotationAfterUpdate.Items.Last();
        quotationAfterUpdate.Items.Should().ContainEquivalentOf(addedItem);

        var total = quotationCreateDto.Items.Sum(p => p.UnitPrice * p.Quantity);
        quotationAfterUpdate.Total.Should().Be(total);
        quotationAfterUpdate.Items.Should().Contain(p => p.ItemName == newItem.Name);
    }

    [Fact]
    public async void Should_UpdateQuotationItem_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var quotationCreateDto = QuotationFaker.GetFakeQuotationCreateDto(itemOutputDtos);
        quotationCreateDto.HospitalId = hospitalOutputDto.Data.Id;
        quotationCreateDto.InsuranceCompanyId = insuranceCompanyOutputDto.Data.Id;
        quotationCreateDto.PhysicianId = physicianOutputDto.Data.Id;
        quotationCreateDto.InternalSpecialistId = Guid.NewGuid();
        quotationCreateDto.PatientId = patientOutputDto.Data.Id;
        quotationCreateDto.PayingSourceType = Domain.Enums.PayingSourceType.Hospital;
        quotationCreateDto.PayingSourceId = hospitalOutputDto.Data.Id;

        var quotation = (await TestClient.Request("/api/quotations").AsAuthenticated().PostJsonAsync(quotationCreateDto).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;
        var lineIdToUpdate = quotation.Items[0].LineId;

        var totalCreate = quotationCreateDto.Items.Sum(p => p.UnitPrice * p.Quantity);

        //act        
        var quotationUpdateItemDto = new QuotationUpdateLineItemDto()
        {
            QuotationId = quotation.Id,
            LineId = lineIdToUpdate,
            LineOrder = quotation.Items[0].LineOrder,
            ItemCode = quotation.Items[1].ItemCode,
            Quantity = quotation.Items[1].Quantity,
            UnitPrice = quotation.Items[1].UnitPrice,
        };

        var updateQuotationResponse = await TestClient.Request($"/api/quotations/{quotation.Id}/items/{quotation.Items[0].LineId}").AsAuthenticated().PutJsonAsync(quotationUpdateItemDto);
        var quotationAfterUpdate = (await TestClient.Request($"/api/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>()).Data;

        //assert
        updateQuotationResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        var updatedItem = quotationAfterUpdate.Items.Single(p => p.LineId == lineIdToUpdate);

        quotationAfterUpdate.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));

        var total = totalCreate - (quotation.Items[0].Quantity * quotation.Items[0].UnitPrice) + (quotation.Items[1].Quantity * quotation.Items[1].UnitPrice);
        quotationAfterUpdate.Total.Should().Be(total);
        updatedItem.ItemName.Should().Be(itemOutputDtos[1].Data.Name);
    }

    [Fact]
    public async void Should_DeleteQuotationItem_When_ValidDataProvided()
    {
        //arrange
        await Seed();

        var quotationCreateDto = QuotationFaker.GetFakeQuotationCreateDto(itemOutputDtos);
        quotationCreateDto.HospitalId = hospitalOutputDto.Data.Id;
        quotationCreateDto.InsuranceCompanyId = insuranceCompanyOutputDto.Data.Id;
        quotationCreateDto.PhysicianId = physicianOutputDto.Data.Id;
        quotationCreateDto.InternalSpecialistId = Guid.NewGuid();
        quotationCreateDto.PatientId = patientOutputDto.Data.Id;
        quotationCreateDto.PayingSourceType = Domain.Enums.PayingSourceType.Hospital;
        quotationCreateDto.PayingSourceId = hospitalOutputDto.Data.Id;

        var quotation = (await TestClient.Request("/api/quotations").AsAuthenticated().PostJsonAsync(quotationCreateDto).ReceiveJson<ResponseDto<QuotationOutputDto>>()).Data;

        //act

        var updateQuotationResponse = await TestClient.Request($"/api/quotations/{quotation.Id}/items/{quotation.Items[0].LineId}").AsAuthenticated().DeleteAsync();
        var quotationAfterUpdateResponse = await TestClient.Request($"/api/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>();
        var quotationAfterUpdate = quotationAfterUpdateResponse.Data;

        //assert
        updateQuotationResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        quotationAfterUpdate.Items.Should().HaveCount(quotation.Items.Count - 1);

        quotationAfterUpdate.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));

        quotationAfterUpdate.Items.Should().NotContain(quotation.Items[0]);
    }

    private async Task Seed()
    {
        var fakeItems = ItemFaker.GetFakerItemCreateDto().Generate(5);
        foreach (var fakeItem in fakeItems)
        {
            var response = await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>();
            itemOutputDtos.Add(response);
        }

        var fakeHospital = HospitalFaker.GetFakeHospitalCreateDto();
        hospitalOutputDto = await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>();

        var fakePatient = PatientFaker.GetFakePatientCreateDto();
        patientOutputDto = await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>();

        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompanyCreateDto();
        insuranceCompanyOutputDto = await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>();

        var fakePhysician = PhysicianFaker.GetFakePhysicianCreateDto();
        physicianOutputDto = await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakePhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>();
    }
}