// using FluentValidation;
// using MediatR;
// using Omini.Opme.Be.Application.Commands;
// using Omini.Opme.Be.Shared.Entities;

// namespace Omini.Opme.Be.Application.PipelineBehaviors;

// public class ValidationBehavior<TRequest, TResponse> :
//     IPipelineBehavior<TRequest, TResponse>
//         where TRequest : class, IRequest<Result<TResponse, ValidationException>>
// {
//     private readonly IEnumerable<IValidator<TRequest>> _validators;

//     public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
//     {
//         _validators = validators;
//     }


//     public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//     {
//         var context = new ValidationContext<TRequest>(request);
//         var failures = _validators.Select(x => x.Validate(context)).SelectMany(x => x.Errors).Where(x => x is not null).ToList();

//             // if (failures.Any())
//             // {
//             //     return new ValidationException(failures);
//             // }

//         return await next();
//     }
// }