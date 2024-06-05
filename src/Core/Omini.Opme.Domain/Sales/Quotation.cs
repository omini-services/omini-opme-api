using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Exceptions;

namespace Omini.Opme.Domain.Sales;

public sealed class Quotation : DocumentEntity
{
    public string PatientCode { get; private set; }
    public string PatientName { get; private set; }
    public Patient Patient { get; private set; }
    public string PhysicianCode { get; private set; }
    public string PhysicianName { get; private set; }
    public Physician Physician { get; private set; }
    public string HospitalCode { get; private set; }
    public string HospitalName { get; private set; }
    public Hospital Hospital { get; private set; }
    public string InsuranceCompanyCode { get; private set; }
    public string InsuranceCompanyName { get; private set; }
    public InsuranceCompany InsuranceCompany { get; private set; }
    public string InternalSpecialistCode { get; private set; }
    public PayingSourceType PayingSourceType { get; private set; }
    public string PayingSourceCode { get; private set; }
    public string PayingSourceName { get; private set; }
    public PayingSource PayingSource { get; set; }
    // public InternalSpecialist InternalSpecialist { get; private set; }
    private DateTime _dueDate;
    public DateTime DueDate { get { return _dueDate; } private set { _dueDate = value.ToUniversalTime(); } }
    private List<QuotationItem> _items = [];
    public IReadOnlyCollection<QuotationItem> Items => _items;
    public decimal Total { get; private set; }

    private Quotation() { }

    public Quotation(string patientCode, string patientName,
                     string physicianCode, string physicianName,
                     string hospitalCode, string hospitalName,
                     string insuranceCompanyCode, string insuranceCompanyName,
                     string internalSpecialistCode,
                     PayingSourceType payingSourceType, string payingSourceCode, string payingSourceName,
                     DateTime dueDate)
    {
        SetData(
            patientCode, patientName,
            physicianCode, physicianName,
            hospitalCode, hospitalName,
            insuranceCompanyCode, insuranceCompanyName,
            internalSpecialistCode,
            payingSourceType, payingSourceCode, payingSourceName,
            dueDate
        );

        SetTotal();
    }

    public void SetData(string patientCode, string patientName,
                        string physicianCode, string physicianName,
                        string hospitalCode, string hospitalName,
                        string insuranceCompanyCode, string insuranceCompanyName,
                        string internalSpecialistCode,
                        PayingSourceType payingSourceType, string payingSourceCode, string payingSourceName,
                        DateTime dueDate)
    {
        PatientCode = patientCode;
        PatientName = patientName;
        PhysicianCode = physicianCode;
        PhysicianName = physicianName;
        HospitalCode = hospitalCode;
        HospitalName = hospitalName;
        InsuranceCompanyCode = insuranceCompanyCode;
        InsuranceCompanyName = insuranceCompanyName;
        InternalSpecialistCode = internalSpecialistCode;
        PayingSourceType = payingSourceType;
        PayingSourceCode = payingSourceCode;
        PayingSourceName = payingSourceName;
        DueDate = dueDate;

        SetTotal();
    }

    public void AddItem(string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, decimal unitPrice, decimal quantity, int? lineOrder = null)
    {
        var newItem = new QuotationItem(
            quotationId: Id,
            lineOrder: lineOrder ?? LastLineOrder,
            lineId: LastLineId,
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

    public QuotationItem(Guid quotationId, int lineId, string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, decimal unitPrice, decimal quantity, int? lineOrder = null)
    {
        QuotationId = quotationId;
        LineId = lineId;

        SetData(itemCode, itemName, referenceCode, anvisaCode, anvisaDueDate, unitPrice, quantity, lineOrder);
    }

    public void SetData(string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, decimal unitPrice, decimal quantity, int? lineOrder = null)
    {
        if (lineOrder is not null)
        {
            LineOrder = lineOrder.Value;
        }

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