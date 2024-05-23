using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Commands;

public record DeleteQuotationCommand : ICommand<Quotation>
{
    public Guid Id { get; init; }

    public class DeleteQuotationCommandHandler : ICommandHandler<DeleteQuotationCommand, Quotation>
    {
        private readonly IQuotationService _quotationService;

        public DeleteQuotationCommandHandler(IQuotationService hospitalService)
        {
            _quotationService = hospitalService;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(DeleteQuotationCommand request, CancellationToken cancellationToken)
        {
            var quotation = await _quotationService.GetById(request.Id, cancellationToken);
            if (quotation is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            await _quotationService.Delete(quotation.Id, cancellationToken);

            return quotation;
        }
    }
}