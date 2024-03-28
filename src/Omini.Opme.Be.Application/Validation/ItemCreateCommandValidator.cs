using FluentValidation;
using Omini.Opme.Be.Application.Commands;

namespace Omini.Opme.Be.Application.Validation;

public class ItemCreateCommandValidator : AbstractValidator<ItemCreateCommand>{
    public ItemCreateCommandValidator()
    {
        RuleFor(x=> x.Name)
            .NotEmpty();
    }
}