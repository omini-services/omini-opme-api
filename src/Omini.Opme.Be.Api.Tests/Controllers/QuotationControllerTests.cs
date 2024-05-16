using Bogus;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Api.Tests.Extensions;

namespace Omini.Opme.Be.Api.Tests.Controllers;

public class QuotationControllerTests : IntegrationTest
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
        quotationCreateDto.PayingSourceType = Domain.Enums.PayingSourceType.Hospital;
        quotationCreateDto.PayingSourceId = hospitalOutputDto.Data.Id;

        //act
        var response = await TestClient.Request("/api/quotations").AsAuthenticated().PostJsonAsync(quotationCreateDto);

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        var quotationOutputDto = await response.GetJsonAsync<ResponseDto<QuotationOutputDto>>();
        quotationCreateDto.Should().BeEquivalentTo(quotationOutputDto.Data,
            options =>
                options.Excluding(p => p.Id)
                    .For(p => p.Items)
                    .Exclude(p => p.ItemTotal)
                    .For(p => p.Items)
                    .Exclude(p => p.LineId)
                    .For(p => p.Items)
                    .Exclude(p => p.LineOrder)
        );


        quotationOutputDto.Data.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));
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

        var fakeItem = ItemFaker.GetFakerItem().Generate();
        var newItem = (await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        //act        
        var faker = new Faker();
        var quotationCreateItemDto = new QuotationCreateItemDto()
        {
            QuotationId = quotation.Id,
            AnvisaCode = newItem.AnvisaCode,
            AnvisaDueDate = newItem.AnvisaDueDate,
            ItemCode = newItem.Code,
            ItemId = newItem.Id,
            Quantity = faker.Random.Number(0, 100),
            UnitPrice = faker.Random.Double(0, 100),
        };

        var updateQuotationResponse = await TestClient.Request($"/api/quotations/{quotation.Id}/items").AsAuthenticated().PostJsonAsync(quotationCreateItemDto);
        var quotationAfterUpdateResponse = await TestClient.Request($"/api/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>();
        var quotationAfterUpdate = quotationAfterUpdateResponse.Data;

        //assert
        updateQuotationResponse.StatusCode.Should().Be(StatusCodes.Status201Created);

        quotationAfterUpdate.Items.Should().HaveCount(quotation.Items.Count + 1);

        quotationAfterUpdate.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));

        var addedItem = quotationAfterUpdate.Items.Last();
        quotationAfterUpdate.Items.Should().ContainEquivalentOf(addedItem,
            options =>
                options.ExcludingMissingMembers()
                    .Excluding(p => p.AnvisaDueDate)
        );
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

        //act        
        var quotationUpdateItemDto = new QuotationUpdateItemDto()
        {
            QuotationId = quotation.Id,
            LineId = lineIdToUpdate,
            LineOrder = quotation.Items[0].LineOrder,
            AnvisaCode = quotation.Items[1].AnvisaCode,
            AnvisaDueDate = quotation.Items[1].AnvisaDueDate,
            ItemCode = quotation.Items[1].ItemCode,
            ItemId = quotation.Items[1].ItemId,
            Quantity = quotation.Items[1].Quantity,
            UnitPrice = quotation.Items[1].UnitPrice,
        };

        var updateQuotationResponse = await TestClient.Request($"/api/quotations/{quotation.Id}/items/{quotation.Items[0].LineId}").AsAuthenticated().PutJsonAsync(quotationUpdateItemDto);
        var quotationAfterUpdateResponse = await TestClient.Request($"/api/quotations/{quotation.Id}").AsAuthenticated().GetJsonAsync<ResponseDto<QuotationOutputDto>>();
        var quotationAfterUpdate = quotationAfterUpdateResponse.Data;

        //assert
        updateQuotationResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        var updatedItem = quotationAfterUpdate.Items.Single(p => p.LineId == lineIdToUpdate);
        quotationUpdateItemDto.Should().BeEquivalentTo(updatedItem,
            options =>
                options.ExcludingMissingMembers()
                    .Excluding(p => p.AnvisaDueDate)
        );

        quotationAfterUpdate.Items.Should()
                                     .OnlyHaveUniqueItems(p => p.LineId).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));
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
        var fakeItems = ItemFaker.GetFakerItem().Generate(5);
        foreach (var fakeItem in fakeItems)
        {
            var response = await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>();
            itemOutputDtos.Add(response);
        }

        var fakeHospital = HospitalFaker.GetFakeHospital();
        hospitalOutputDto = await TestClient.Request("/api/hospitals").AsAuthenticated().PostJsonAsync(fakeHospital).ReceiveJson<ResponseDto<HospitalOutputDto>>();

        var fakePatient = PatientFaker.GetFakePatient();
        patientOutputDto = await TestClient.Request("/api/patients").AsAuthenticated().PostJsonAsync(fakePatient).ReceiveJson<ResponseDto<PatientOutputDto>>();

        var fakeInsuranceCompany = InsuranceCompanyFaker.GetFakeInsuranceCompany();
        insuranceCompanyOutputDto = await TestClient.Request("/api/insurancecompanies").AsAuthenticated().PostJsonAsync(fakeInsuranceCompany).ReceiveJson<ResponseDto<InsuranceCompanyOutputDto>>();

        var fakePhysician = PhysicianFaker.GetFakePhysician();
        physicianOutputDto = await TestClient.Request("/api/physicians").AsAuthenticated().PostJsonAsync(fakePhysician).ReceiveJson<ResponseDto<PhysicianOutputDto>>();
    }
}