using Omini.Opme.Be.Domain.Enums;

namespace Omini.Opme.Be.Api.Dtos;

public sealed record QuotationCreateDto
{
    public string Number { get; set; }
    public Guid PatientId { get; set; }
    public Guid PhysicianId { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public Guid PayingSourceId { get; set; }
    public Guid HospitalId { get; set; }
    public Guid InsuranceCompanyId { get; set; }
    public Guid InternalSpecialistId { get; set; }
    public DateTime DueDate { get; set; }
    public List<QuotationCreateItemDto> Items { get; set; } = new();

    public sealed record QuotationCreateItemDto
    {
        public int? LineOrder { get; set; }
        public string ItemCode { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
    }
}

public sealed record QuotationCreateLineItemDto
{
    public Guid QuotationId { get; set; }
    public int? LineOrder { get; set; }
    public string ItemCode { get; set; }
    public double UnitPrice { get; set; }
    public double Quantity { get; set; }
}

public sealed record QuotationUpdateDto
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Guid PatientId { get; set; }
    public Guid PhysicianId { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public Guid PayingSourceId { get; set; }
    public Guid HospitalId { get; set; }
    public Guid InsuranceCompanyId { get; set; }
    public Guid InternalSpecialistId { get; set; }
    public DateTime DueDate { get; set; }
    public List<QuotationUpdateItemDto> Items { get; set; } = new();

    public sealed record QuotationUpdateItemDto
    {
        public int LineId { get; set; }
        public int? LineOrder { get; set; }
        public string ItemCode { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
    }
}

public sealed record QuotationUpdateLineItemDto
{
    public Guid QuotationId { get; set; }
    public int LineId { get; set; }
    public int? LineOrder { get; set; }
    public string ItemCode { get; set; }
    public double UnitPrice { get; set; }
    public double Quantity { get; set; }
}

public sealed record QuotationOutputDto
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Guid PatientId { get; set; }
    public Guid PhysicianId { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public Guid PayingSourceId { get; set; }
    public string PayingSource { get; set; }
    public Guid HospitalId { get; set; }
    public Guid InsuranceCompanyId { get; set; }
    public Guid InternalSpecialistId { get; set; }
    public DateTime DueDate { get; set; }
    public List<QuotationOutputItemDto> Items { get; set; }
    public double Total { get; set; }

    public sealed record QuotationOutputItemDto
    {
        public int LineId { get; set; }
        public int LineOrder { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string AnvisaCode { get; set; }
        public DateTime AnvisaDueDate { get; set; }
        public double UnitPrice { get; set; }
        public double ItemTotal { get; set; }
        public double Quantity { get; set; }
    }
}