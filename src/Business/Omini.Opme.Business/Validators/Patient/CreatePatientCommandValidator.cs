using FluentValidation;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Validators.Custom;

namespace Omini.Opme.Business.Validators;

internal class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(x => x.Cpf).Cpf();
    }
}