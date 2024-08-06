using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Domain.ValueObjects;

namespace Omini.Opme.Domain.Sales;

public sealed class Quotation : DocumentEntity
{
    public string PatientCode { get; private set; }
    public PersonName PatientName { get; private set; }
    public string PhysicianCode { get; private set; }
    public PersonName PhysicianName { get; private set; }
    public string HospitalCode { get; private set; }
    public string HospitalName { get; private set; }
    public string InsuranceCompanyCode { get; private set; }
    public string InsuranceCompanyName { get; private set; }
    public string InternalSpecialistCode { get; private set; }
    public PayingSourceType PayingSourceType { get; private set; }
    private DateTime _dueDate;
    public DateTime DueDate { get { return _dueDate; } private set { _dueDate = value.ToUniversalTime(); } }
    private List<QuotationItem> _items = [];
    public IReadOnlyCollection<QuotationItem> Items => _items;
    public double Total { get; private set; }
    public string? Comments { get; set; }

    private Quotation() { }

    public Quotation(string patientCode, PersonName patientName,
                     string physicianCode, PersonName physicianName,
                     string hospitalCode, string hospitalName,
                     string insuranceCompanyCode, string insuranceCompanyName,
                     string internalSpecialistCode,
                     PayingSourceType payingSourceType,
                     DateTime dueDate,
                     string? comments)
    {
        SetData(
            patientCode, patientName,
            physicianCode, physicianName,
            hospitalCode, hospitalName,
            insuranceCompanyCode, insuranceCompanyName,
            internalSpecialistCode,
            payingSourceType,
            dueDate,
            comments
        );

        SetTotal();
    }

    private void SetTotal()
    {
        Total = _items.Any() ? _items.Sum(x => x.LineTotal) : 0;
    }

    private int LastLineOrder => _items.Any() ? _items.Max(p => p.LineOrder) + 1 : 0;
    private int LastLineId => _items.Any() ? _items.Max(p => p.LineId) + 1 : 0;

    public void SetData(string patientCode, PersonName patientName,
                        string physicianCode, PersonName physicianName,
                        string hospitalCode, string hospitalName,
                        string insuranceCompanyCode, string insuranceCompanyName,
                        string internalSpecialistCode,
                        PayingSourceType payingSourceType,
                        DateTime dueDate,
                        string? comments)
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
        DueDate = dueDate;
        Comments = comments;

        SetTotal();
    }

    public void AddItem(string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, double unitPrice, double quantity, int? lineOrder = null)
    {
        var newItem = new QuotationItem(
            documentID: Id,
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

        if (item.DocumentId != Id || item.LineId != lineId)
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
}

public sealed class QuotationItem : DocumentRowEntity
{
    public string ItemCode { get; private set; }
    public string ItemName { get; private set; }
    public string ReferenceCode { get; private set; }
    public string AnvisaCode { get; private set; }
    private DateTime _anvisaDueDate;
    public DateTime AnvisaDueDate { get { return _anvisaDueDate; } private set { _anvisaDueDate = value.ToUniversalTime(); } }
    public double UnitPrice { get; private set; }
    public double Quantity { get; private set; }
    public double LineTotal { get; private set; }
    private double GetLineTotal => Quantity * UnitPrice;

    private QuotationItem() { }

    public QuotationItem(Guid documentID, int lineId, string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, double unitPrice, double quantity, int? lineOrder = null)
    {
        DocumentId = documentID;
        LineId = lineId;

        SetData(itemCode, itemName, referenceCode, anvisaCode, anvisaDueDate, unitPrice, quantity, lineOrder);
    }

    public void SetData(string itemCode, string itemName, string referenceCode, string anvisaCode, DateTime anvisaDueDate, double unitPrice, double quantity, int? lineOrder = null)
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