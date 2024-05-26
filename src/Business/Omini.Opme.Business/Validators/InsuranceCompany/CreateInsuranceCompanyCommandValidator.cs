using FluentValidation;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Validators.Custom;

namespace Omini.Opme.Business.Validators;

internal class CreateInsuranceCompanyCommandValidator : AbstractValidator<CreateInsuranceCompanyCommand>
{
    public CreateInsuranceCompanyCommandValidator()
    {
        RuleFor(x => x.Cnpj).Cnpj();
    }
}