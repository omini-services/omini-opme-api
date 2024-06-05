using Omini.Opme.Domain.Common;

namespace Omini.Opme.Api.Dtos;

public sealed record QuotationOutputDto
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public string PatientCode { get; set; }
    public string PatientName { get; set; }
    public string PhysicianCode { get; set; }
    public string PhysicianName { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public string PayingSourceCode { get; set; }
    public string PayingSourceName { get; set; }
    public string HospitalCode { get; set; }
    public string HospitalName { get; set; }
    public string InsuranceCompanyCode { get; set; }
    public string InsuranceCompanyName { get; set; }
    public string InternalSpecialistCode { get; set; }
    public string InternalSpecialistName { get; set; }
    public DateTime DueDate { get; set; }
    public List<QuotationOutputItemDto> Items { get; set; }
    public decimal Total { get; set; }

    public sealed record QuotationOutputItemDto
    {
        public int LineId { get; set; }
        public int LineOrder { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string AnvisaCode { get; set; }
        public DateTime AnvisaDueDate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal Quantity { get; set; }
    }
}