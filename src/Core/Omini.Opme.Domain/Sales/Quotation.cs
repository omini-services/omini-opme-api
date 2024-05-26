using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Entities;
using Omini.Opme.Domain.Exceptions;

namespace Omini.Opme.Domain.Sales;

public sealed class Quotation : SoftDeletable
{
    public long Number { get; private set; }
    public Guid PatientId { get; private set; }
    public Patient Patient { get; private set; }
    public Guid PhysicianId { get; private set; }
    public Physician Physician { get; private set; }
    public PayingSourceType PayingSourceType { get; private set; }
    public Guid PayingSourceId { get; private set; }
    public PayingSource PayingSource { get; set; }
    public Guid HospitalId { get; private set; }
    public Hospital Hospital { get; private set; }
    public Guid InsuranceCompanyId { get; private set; }
    public InsuranceCompany InsuranceCompany { get; private set; }
    public Guid InternalSpecialistId { get; private set; }
    // public InternalSpecialist InternalSpecialist { get; private set; }
    private DateTime _dueDate;
    public DateTime DueDate { get { return _dueDate; } private set { _dueDate = value.ToUniversalTime(); } }
    private List<QuotationItem> _items = [];
    public IReadOnlyCollection<QuotationItem> Items => _items;
    public decimal Total { get; private set; }

    private Quotation() { }

    public Quotation(Guid patientId, Guid physicianId, PayingSourceType payingSourceType, Guid payingSourceId, Guid hospitalId, Guid insuranceCompanyId, Guid internalSpecialistId, DateTime dueDate)
    {
        SetData(patientId, physicianId, payingSourceType, payingSourceId, hospitalId, insuranceCompanyId, internalSpecialistId, dueDate);
        SetTotal();
    }

    public void SetData(Guid patientId, Guid physicianId, PayingSourceType payingSourceType, Guid payingSourceId, Guid hospitalId, Guid insuranceCompanyId, Guid internalSpecialistId, DateTime dueDate)
    {
        PatientId = patientId;
        PhysicianId = physicianId;
        PayingSourceType = payingSourceType;
        PayingSourceId = payingSourceId;
        HospitalId = hospitalId;
        InsuranceCompanyId = insuranceCompanyId;
        InternalSpecialistId = internalSpecialistId;
        DueDate = dueDate;
        SetTotal();
    }

    public void AddItem(Guid itemId, string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, decimal unitPrice, decimal quantity, int? lineOrder = null)
    {
        var newItem = new QuotationItem(
            quotationId: Id,
            lineOrder: lineOrder ?? LastLineOrder,
            lineId: LastLineId,
            itemId: itemId,
            itemCode: itemCode,
            itemName: itemName,
            referenceCode: referenceCode,
            anvisaCode: anvisaCode,
            anvisaDueDate: anvisaDueDate,
            unitPrice: unitPrice,
            quantity: quantity
        );

        _items.Add(newItem);
        SetTotal();
    }

    public void UpdateItem(int lineId, Action<QuotationItem> updateAction)
    {
        var item = _items.SingleOrDefault(i => i.LineId == lineId);
        if (item == null)
            throw new InvalidItemProvidedException();

        updateAction(item);

        if (item.QuotationId != Id || item.LineId != lineId)
        {
            throw new MissMatchIdException();
        }

        SetTotal();
    }

    public void RemoveItem(QuotationItem item)
    {
        _items.Remove(item);
        SetTotal();
    }

    private void SetTotal()
    {
        Total = _items.Any() ? _items.Sum(x => x.LineTotal) : 0;
    }

    private int LastLineOrder => _items.Any() ? _items.Max(p => p.LineOrder) + 1 : 0;
    private int LastLineId => _items.Any() ? _items.Max(p => p.LineId) + 1 : 0;
}

public sealed class QuotationItem
{
    public Guid QuotationId { get; private set; }
    public int LineId { get; private set; }
    public int LineOrder { get; private set; }
    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; }
    public string ItemName { get; private set; }
    public string ReferenceCode { get; private set; }
    public string AnvisaCode { get; private set; }
    private DateTime _anvisaDueDate;
    public DateTime AnvisaDueDate { get { return _anvisaDueDate; } private set { _anvisaDueDate = value.ToUniversalTime(); } }
    public decimal UnitPrice { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal LineTotal { get; private set; }
    private decimal GetLineTotal => Quantity * UnitPrice;

    private QuotationItem() { }

    public QuotationItem(Guid quotationId, Guid itemId, int lineId, string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, decimal unitPrice, decimal quantity, int? lineOrder = null)
    {
        QuotationId = quotationId;
        LineId = lineId;

        SetData(itemId, itemCode, itemName, referenceCode, anvisaCode, anvisaDueDate, unitPrice, quantity, lineOrder);
    }

    public void SetData(Guid itemId, string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, decimal unitPrice, decimal quantity, int? lineOrder = null)
    {
        if (lineOrder is not null)
        {
            LineOrder = lineOrder.Value;
        }

        ItemId = itemId;
        ItemCode = itemCode;
        ItemName = itemName;
        ReferenceCode = referenceCode;
        AnvisaCode = anvisaCode;
        AnvisaDueDate = anvisaDueDate;
        UnitPrice = unitPrice;
        Quantity = quantity;
        LineTotal = GetLineTotal;
    }
}