using Omini.Opme.Domain.Common;

namespace Omini.Opme.Api.Dtos;

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