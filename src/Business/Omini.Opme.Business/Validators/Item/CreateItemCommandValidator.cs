using FluentValidation;
using Omini.Opme.Business.Commands;

namespace Omini.Opme.Business.Validators;

internal class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}