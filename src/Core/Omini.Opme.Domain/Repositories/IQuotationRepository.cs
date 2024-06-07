using Omini.Opme.Domain.Sales;

namespace Omini.Opme.Domain.Repositories;

public interface IQuotationRepository : IRespositoryDocumentEntity<Quotation>
{
}

public interface IQuotationItemRepository : IRespositoryDocumentRowEntity<QuotationItem>
{
}