using FluentValidation;
using Omini.Opme.Business.Commands;
using Omini.Opme.Business.Validators.Custom;

namespace Omini.Opme.Business.Validators;

internal class UpdateInsuranceCompanyCommandValidator : AbstractValidator<UpdateInsuranceCompanyCommand>
{
    public UpdateInsuranceCompanyCommandValidator()
    {
        RuleFor(x => x.Cnpj).Cnpj();
    }
}