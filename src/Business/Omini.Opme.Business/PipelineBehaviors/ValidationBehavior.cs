using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationFailures = _validators.Select(x => x.Validate(context)).SelectMany(x => x.Errors).Where(x => x is not null).ToArray();

        if (!validationFailures.Any())
        {
            return await next();
        }

        return CreateValidationResult<TResponse>(new ValidationResult(validationFailures));
    }

    public static TResponse CreateValidationResult<TResult>(ValidationResult validationResult)
    {
        Type resultTypeDefinition = typeof(Result<,>).GetGenericTypeDefinition();
        Type[] typeArguments = typeof(TResult).GenericTypeArguments;
        Type constructedType = resultTypeDefinition.MakeGenericType(typeArguments);
        var implicitOperator = constructedType.GetMethod("op_Implicit", [typeof(ValidationResult)]);
        object convertedInstance = implicitOperator.Invoke(null, [validationResult])!;
        return (convertedInstance as TResponse)!;
    }
}