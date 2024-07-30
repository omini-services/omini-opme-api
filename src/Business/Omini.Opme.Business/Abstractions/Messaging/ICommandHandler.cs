using FluentValidation.Results;
using MediatR;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse, ValidationResult>>
    where TCommand : ICommand<TResponse>
{

}