using FluentValidation.Results;
using Omini.Opme.Application.Abstractions.Messaging;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Domain.Services;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Be.Application.Commands;

public record PreviewQuotationCommand : ICommand<byte[]>
{
    public Guid Id { get; set; }

    public class PreviewQuotationHandler : ICommandHandler<PreviewQuotationCommand, byte[]>
    {
        private readonly IQuotationPdfGenerator _quotationPdfGenerator;
        private readonly IQuotationService _quotationService;
        public PreviewQuotationHandler(IQuotationPdfGenerator quotationPdfGenerator, IQuotationService quotationService)
        {
            _quotationPdfGenerator = quotationPdfGenerator;
            _quotationService = quotationService;
        }

        public async Task<Result<byte[], ValidationResult>> Handle(PreviewQuotationCommand request, CancellationToken cancellationToken)
        {
            var validationFailures = new List<ValidationFailure>();
            var quotation = await _quotationService.GetById(request.Id);
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