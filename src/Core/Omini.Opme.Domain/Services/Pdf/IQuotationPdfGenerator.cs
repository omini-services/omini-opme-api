using Omini.Opme.Domain.Sales;

namespace Omini.Opme.Domain.Services.Pdf;

public interface IQuotationPdfGenerator
{
    byte[] GenerateBytes(Quotation quotation);
}
