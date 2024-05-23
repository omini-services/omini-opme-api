using FluentValidation.Results;
using MediatR;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse, ValidationResult>>
    where TQuery : IQuery<TResponse>
{
}