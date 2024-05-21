using FluentValidation.Results;
using Omini.Opme.Be.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Repositories;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record PreviewQuotationCommand : ICommand<byte[]>
{
    public Guid Id { get; set; }

    public class PreviewQuotationHandler : ICommandHandler<PreviewQuotationCommand, byte[]>
    {
        private readonly IQuotationPdfGenerator _quotationPdfGenerator;
        private readonly IQuotationRepository _quotationRepository;
        public PreviewQuotationHandler(IQuotationPdfGenerator quotationPdfGenerator, IQuotationRepository quotationRepository)
        {
            _quotationPdfGenerator = quotationPdfGenerator;
            _quotationRepository = quotationRepository;
        }

        public async Task<Result<byte[], ValidationResult>> Handle(PreviewQuotationCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var quotation = await _quotationRepository.GetById(request.Id);
            if (quotation is null)
            {
                validationFailures.Add(new ValidationFailure(nameof(request.Id), "Invalid Id"));
            }

            if (validationFailures.Any())
            {
                return new ValidationResult(validationFailures);
            }

            var document = _quotationPdfGenerator.GenerateBytes(quotation!);

            return document;
        }
    }
}