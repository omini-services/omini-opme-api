using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Entities;

namespace Omini.Opme.Domain.Sales;

public sealed class Quotation : Auditable
{
    public string Number { get; init; }
    public Guid PatientId { get; init; }
    public Patient Patient { get; init; }
    public Guid PhysicianId { get; init; }
    public Physician Physician { get; init; }
    public PayingSourceType PayingSourceType { get; init; }
    public Guid PayingSourceId { get; init; }
    public PayingSource PayingSource { get; set; }
    public Guid HospitalId { get; init; }
    public Hospital Hospital { get; init; }
    public Guid InsuranceCompanyId { get; init; }
    public InsuranceCompany InsuranceCompany { get; init; }
    public Guid InternalSpecialistId { get; init; }
    // public InternalSpecialist InternalSpecialist { get; init; }
    public DateTime DueDate { get; init; }
    private List<QuotationItem> _items = new();
    public IReadOnlyCollection<QuotationItem> Items => _items;
    public double Total { get; private set; }

    public void AddItem(QuotationItem item)
    {
        _items.Add(item);
        Total += item.ItemTotal;
    }

    public void RemoveItem(QuotationItem item)
    {
        _items.Remove(item);
        Total -= item.ItemTotal;
    }
}

public sealed class QuotationItem
{
    public Guid QuotationId { get; init; }
    public int LineId { get; init; }
    public int? LineOrder { get; init; }
    public Guid ItemId { get; init; }
    public string ItemCode { get; init; }
    public string ItemName { get; init; }
    public string ReferenceCode { get; init; }
    public string AnvisaCode { get; init; }
    public DateTime AnvisaDueDate { get; init; }
    private double _unitPrice;
    public double UnitPrice { get { return _unitPrice; } init { _unitPrice = value; CalculateTotal(); } }
    private double _quantity;
    public double Quantity { get { return _quantity; } init { _quantity = value; CalculateTotal(); } }
    public double ItemTotal { get; private set; }

    private void CalculateTotal()
    {
        ItemTotal = Quantity * UnitPrice;
    }
}