using MediatR;
using Omini.Opme.Shared.Entities;

namespace Omini.Opme.Business.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, PagedResult<TResponse>>
    where TQuery : IQuery<TResponse>
{

}