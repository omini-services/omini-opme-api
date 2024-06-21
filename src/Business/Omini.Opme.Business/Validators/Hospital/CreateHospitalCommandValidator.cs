using FluentValidation;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Validators.Custom;

namespace Omini.Opme.Business.Validators;

internal class CreateHospitalCommandValidator : AbstractValidator<CreateHospitalCommand>
{
    public CreateHospitalCommandValidator()
    {
        RuleFor(x => x.LegalName).NotEmpty();
        RuleFor(x => x.TradeName).NotEmpty();
        RuleFor(x => x.Cnpj).Cnpj();
    }
}