using FluentValidation.Results;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Domain.Services;

public interface IQuotationService
{
    public Task<Quotation> Add(Quotation quotation, CancellationToken cancellationToken = default);
    public Task<Quotation> Update(Quotation quotation, CancellationToken cancellationToken = default);
    public Task<Quotation> Delete(Guid id, CancellationToken cancellationToken = default);
    public Task<Quotation> GetById(Guid id, CancellationToken cancellationToken = default);
}