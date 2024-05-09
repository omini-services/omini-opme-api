using FluentValidation.Results;
using MediatR;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse, ValidationResult>>
    where TCommand : ICommand<TResponse>
{

}