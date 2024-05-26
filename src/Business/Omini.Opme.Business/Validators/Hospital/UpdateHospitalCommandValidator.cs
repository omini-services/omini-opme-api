using FluentValidation;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Validators.Custom;

namespace Omini.Opme.Business.Validators;

internal class UpdateHospitalCommandValidator : AbstractValidator<UpdateHospitalCommand>
{
    public UpdateHospitalCommandValidator()
    {
        RuleFor(x => x.Cnpj).Cnpj();
    }
}