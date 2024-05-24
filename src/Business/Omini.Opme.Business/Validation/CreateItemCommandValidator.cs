using FluentValidation;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Business.Validation;

internal class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}