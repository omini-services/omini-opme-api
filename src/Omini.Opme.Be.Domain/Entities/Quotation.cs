using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Domain.Entities;

public class Quotation : Auditable
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Guid PacientId { get; set; }
    public Guid PhysicianId { get; set; }
    public string PayingSource { get; set; }
    public Guid PayingSourceI { get; set; }
    public Guid HospitalId { get; set; }
    public Guid InsuranceSupplierId { get; set; }
    public Guid SpecialistId { get; set; }
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