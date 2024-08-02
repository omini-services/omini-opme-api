using FluentValidation;
using Omini.Opme.Shared.Formatters;

namespace Omini.Opme.Business.Validators.Custom;

public static class CpfValidator
{
  public static IRuleBuilderOptions<T, string> Cpf<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(m =>
    {
      try
      {
        Formatters.FormatCpf(m);
        return true;
      }
      catch (FormatException)
      {
        return false;
      }
    }).WithMessage("Invalid CPF");
  }
}