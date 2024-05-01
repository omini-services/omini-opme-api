using Omini.Opme.Be.Domain.Enums;

namespace Omini.Opme.Be.Domain.Entities;

public class Quotation : Entity
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
    public List<QuotationItem> Items { get; set; }
}

public class QuotationItem
{
    public Guid QuotationId { get; set; }
    public int LineId { get; set; }
    public int Order { get; set; }
    public Guid ItemId { get; set; }
    public string ItemCode { get; set; }
    public string AnvisaCode { get; set; }
    public DateTime AnvisaDueDate { get; set; }
    public double UnitPrice { get; set; }
    public double ItemTotal { get; set; }
    public double Quantity { get; set; }
}