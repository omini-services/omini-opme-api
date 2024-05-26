using FluentValidation;
using Omini.Opme.Shared;

namespace Omini.Opme.Business.Validators.Custom;

public static class CnpjValidator
{
  public static IRuleBuilderOptions<T, string> Cnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(m =>
    {
      try
      {
        Formatters.FormatCnpj(m);
        return true;
      }
      catch (FormatException)
      {
        return false;
      }
    }).WithMessage("Invalid CNPJ");
  }
}