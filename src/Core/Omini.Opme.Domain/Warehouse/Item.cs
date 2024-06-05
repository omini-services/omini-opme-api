using Omini.Opme.Domain.Common;

namespace Omini.Opme.Domain.Warehouse;

public sealed class Item : MasterEntity
{
    private Item() { }
    
    public Item(string code,
        string name,
        string? salesName,
        string description,
        string? uom,
        string? anvisaCode,
        DateTime? anvisaDueDate,
        string? supplierCode,
        string? cst,
        string? susCode,
        string? ncmCode)
    {
        SetData(code, name, salesName, description, uom, anvisaCode, anvisaDueDate, supplierCode, cst, susCode, ncmCode);
    }

    public string? SalesName { get; private set; }
    public string Description { get; private set; }
    public string? Uom { get; private set; }
    public string? AnvisaCode { get; private set; }
    public DateTime? AnvisaDueDate { get; private set; }
    public string? SupplierCode { get; private set; }
    public string? Cst { get; private set; }
    public string? SusCode { get; private set; }
    public string? NcmCode { get; private set; }

    public Item SetData(string code, string name, string? salesName, string description, string? uom, string? anvisaCode, DateTime? anvisaDueDate, string? supplierCode, string? cst, string? susCode, string? ncmCode)
    {
        Code = code;
        Name = name;
        SalesName = salesName;
        Description = description;
        Uom = uom;
        AnvisaCode = anvisaCode;
        AnvisaDueDate = anvisaDueDate;
        SupplierCode = supplierCode;
        Cst = cst;
        SusCode = susCode;
        NcmCode = ncmCode;

        return this;
    }
}