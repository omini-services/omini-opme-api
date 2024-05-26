using FluentValidation;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Validators.Custom;

namespace Omini.Opme.Business.Validators;

internal class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientCommandValidator()
    {
        RuleFor(x => x.Cpf).Cpf();
    }
}