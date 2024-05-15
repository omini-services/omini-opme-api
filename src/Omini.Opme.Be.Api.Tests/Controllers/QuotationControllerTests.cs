using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
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
    private List<ResponseDto<ItemOutputDto>> itemsOutputDto = new();

    [Fact]
    public async void Create_Quotation()
    {
        //arrange
        await Seed();

        var faker = new Faker();

        var quotationCreateDto = new QuotationCreateDto()
        {
            DueDate = faker.Date.Future().AsUtc(),
            HospitalId = hospitalOutputDto.Data.Id,
            InsuranceCompanyId = insuranceCompanyOutputDto.Data.Id,
            PhysicianId = physicianOutputDto.Data.Id,
            InternalSpecialistId = Guid.NewGuid(),
            Number = faker.Random.AlphaNumeric(5),
            PatientId = patientOutputDto.Data.Id,
            PayingSourceType = Domain.Enums.PayingSourceType.Hospital,
            PayingSourceId = hospitalOutputDto.Data.Id,
        };

        foreach (var item in itemsOutputDto)
        {
            quotationCreateDto.Items.Add(new QuotationCreateDto.QuotationCreateItemDto()
            {
                ItemId = item.Data.Id,
                ItemCode = item.Data.Code,
                AnvisaCode = item.Data.AnvisaCode,
                AnvisaDueDate = item.Data.AnvisaDueDate,
                Quantity = faker.Random.Number(1, 100),
                UnitPrice = faker.Random.Double(max: 1000.0)
            });
        }

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
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .OnlyHaveUniqueItems(p => p.LineOrder).And
                                     .AllSatisfy(p => p.ItemTotal.Should().BeGreaterThanOrEqualTo(0));
    }

    private async Task Seed()
    {
        ItemFaker.GetFakerItem().Generate(5).ForEach(async (i) =>
        {
            var response = await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(i).ReceiveJson<ResponseDto<ItemOutputDto>>();
            itemsOutputDto.Add(response);
        });

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