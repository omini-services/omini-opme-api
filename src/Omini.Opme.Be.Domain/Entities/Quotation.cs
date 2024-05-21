using Omini.Opme.Be.Domain.Enums;

namespace Omini.Opme.Be.Domain.Entities;

public sealed class Quotation : Auditable
{
    public string Number { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; }
    public Guid PhysicianId { get; set; }
    public Physician Physician { get; set; }
    public PayingSourceType PayingSourceType { get; set; }
    public Guid PayingSourceId { get; set; }
    public PayingSource PayingSource { get; set; }
    public Guid HospitalId { get; set; }
    public Hospital Hospital { get; set; }
    public Guid InsuranceCompanyId { get; set; }
    public InsuranceCompany InsuranceCompany { get; set; }
    public Guid InternalSpecialistId { get; set; }
    // public InternalSpecialist InternalSpecialist { get; set; }
    public DateTime DueDate { get; set; }
    public ICollection<QuotationItem> Items { get; set; }    
    public double Total { get; set; }
}

public sealed class QuotationItem
{
    public Guid QuotationId { get; set; }
    public int LineId { get; set; }
    public int? LineOrder { get; set; }
    public Guid ItemId { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string ReferenceCode { get; set; }
    public string AnvisaCode { get; set; }
    public DateTime AnvisaDueDate { get; set; }
    public double UnitPrice { get; set; }
    public double ItemTotal { get; set; }
    public double Quantity { get; set; }
}