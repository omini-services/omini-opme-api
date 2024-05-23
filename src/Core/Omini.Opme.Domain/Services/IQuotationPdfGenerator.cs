using Omini.Opme.Domain.Sales;

namespace Omini.Opme.Domain.Services;

public interface IQuotationPdfGenerator
{
    byte[] GenerateBytes(Quotation quotation);
}
