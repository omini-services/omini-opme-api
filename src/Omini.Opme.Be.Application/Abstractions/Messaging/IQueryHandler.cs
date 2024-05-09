using System.ComponentModel.DataAnnotations;
using MediatR;
using Omini.Opme.Be.Shared.Entities;

namespace Omini.Opme.Be.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse, ValidationResult>>
    where TQuery : IQuery<TResponse>
{

}