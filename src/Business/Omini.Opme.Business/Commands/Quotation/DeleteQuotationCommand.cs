using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Shared.Entities;
using Omini.Opme.Domain.Repositories;
using Omini.Opme.Domain.Transactions;

namespace Omini.Opme.Business.Commands;

public record DeleteQuotationCommand : ICommand<Quotation>
{
    public Guid Id { get; init; }

    public class DeleteQuotationCommandHandler : ICommandHandler<DeleteQuotationCommand, Quotation>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotationRepository _quotationRepository;

        public DeleteQuotationCommandHandler(IUnitOfWork unitOfWork, IQuotationRepository hospitalRepository)
        {
            _unitOfWork = unitOfWork;
            _quotationRepository = hospitalRepository;
        }

        public async Task<Result<Quotation, ValidationResult>> Handle(DeleteQuotationCommand request, CancellationToken cancellationToken)
        {
            var quotation = await _quotationRepository.GetById(request.Id, cancellationToken);
            if (quotation is null)
            {
                return new ValidationResult([new ValidationFailure(nameof(request.Id), "Invalid id")]);
            }

            _quotationRepository.Delete(quotation, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return quotation;
        }
    }
}