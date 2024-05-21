using Omini.Opme.Be.Domain.Entities;

namespace Omini.Opme.Be.Domain.Services;

public interface IQuotationPdfGenerator
{
    byte[] GenerateBytes(Quotation quotation);
}
