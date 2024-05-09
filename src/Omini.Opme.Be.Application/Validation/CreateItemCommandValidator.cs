using FluentValidation;
using Omini.Opme.Be.Application.Commands;

namespace Omini.Opme.Be.Application.Validation;

internal class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}